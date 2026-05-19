# 🔧 Login Flash & Redirect Issue - Fixed

## Problem Description
When logging in with correct credentials, the dashboard briefly appears then redirects back to the login page.

## Root Causes Fixed

### 1. ❌ Too Aggressive 401 Handling
**Issue**: When dashboard API calls (profile, dashboard) returned 401, it immediately cleared the token and redirected to login.
**Fix**: Removed aggressive redirect from API service. Now 401 errors are caught and displayed gracefully.

### 2. ❌ Role-Based Authorization Issues
**Issue**: Customer and Appointment controllers had `[Authorize(Roles = "customer")]` at the class level, preventing any endpoint access if role claim wasn't exactly matching.
**Fix**: Moved `[Authorize]` to class level and `[Authorize(Roles = "customer")]` to individual methods for more flexibility.

### 3. ❌ JWT Role Claim Type Mismatch
**Issue**: JWT was generating with role claims but the server was looking in wrong places.
**Fix**: Updated Program.cs to use `ClaimTypes.Role` consistently and improved token validation logging.

### 4. ❌ Dynamic API URL Issues
**Issue**: Frontend was hardcoded to `http://localhost:5000/api` but app might be running on different port.
**Fix**: Created dynamic `getApiBase()` function that determines the correct API URL based on browser location.

## Changes Made

### Backend Changes

#### 1. `Program.cs` - JWT Configuration
```csharp
// Changed from:
RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"

// To:
RoleClaimType = ClaimTypes.Role
```

Added better logging for debugging:
```csharp
OnTokenValidated = context =>
{
    var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var role = context.Principal?.FindFirst(ClaimTypes.Role)?.Value;
    Console.WriteLine($"✅ Token valid - UserId: {userId}, Role: {role}");
    return Task.CompletedTask;
}
```

#### 2. `CustomerController.cs`
```csharp
// Before:
[Authorize(Roles = "customer")]
public class CustomerController : ControllerBase { }

// After:
[Authorize]
public class CustomerController : ControllerBase { }

// Then on each method:
[HttpGet("dashboard")]
[Authorize(Roles = "customer")]
public async Task<IActionResult> GetDashboard() { }
```

#### 3. `AppointmentController.cs`
Same change as CustomerController - moved authorization from class to method level.

### Frontend Changes

#### 1. `js/api.js` - Dynamic URL & Better Error Handling
```javascript
// Dynamic API URL based on browser location
const getApiBase = () => {
    const protocol = window.location.protocol;
    const host = window.location.hostname;
    const port = window.location.port;

    if (host === 'localhost' && !port) {
        return `${protocol}//localhost:5000/api`;
    }
    if (port) {
        return `${protocol}//${host}:${port}/api`;
    }
    return `${protocol}//${host}/api`;
};

// Get fresh token from localStorage for each request
const token = localStorage.getItem('authToken');
headers['Authorization'] = `Bearer ${token}`;

// Don't immediately redirect on 401 - let the caller handle it
if (response.status === 401) {
    const errorText = await response.text();
    console.error('❌ 401 Unauthorized:', errorText);
    throw new Error('Unauthorized - Token may be expired. Please login again.');
}
```

#### 2. `js/app.js` - Graceful Error Handling
```javascript
// Use Promise.allSettled to handle partial failures
const [dashboardResponse, profileResponse] = await Promise.allSettled([
    CustomerApi.getDashboard(),
    CustomerApi.getProfile()
]);

// Handle each response individually
if (dashboardResponse.status === 'fulfilled') {
    dashboard = dashboardResponse.value;
    console.log('✅ Dashboard loaded:', dashboard);
} else {
    console.warn('⚠️ Dashboard load failed:', dashboardResponse.reason);
}

// Show content even if some requests fail
showContent(`...dashboard HTML...`);
```

## How to Test

### Step 1: Start the API
```bash
cd GarageHub.API
dotnet run
```

### Step 2: Check Console Output
Look for these messages in Visual Studio Output window:
```
🔑 Token received for /api/customer/profile
✅ Token valid - UserId: 1, Role: customer
✅ Database migrations applied
✅ Admin exists (Id: 1) — all good
```

### Step 3: Open Browser
Navigate to:
- HTTP: `http://localhost:5000/`
- HTTPS: `https://localhost:7000/` (if running on HTTPS)

### Step 4: Test Login
1. Open Browser Console (F12)
2. Go to Application/Storage → LocalStorage
3. Login with credentials:
   - Email: `smartsikchya.noreply@gmail.com`
   - Password: `Admin@123456`
4. Or register new account

### Step 5: Check Console Logs
Expected sequence:
```
Attempting login with: user@example.com
✅ Login successful! Token stored: eyJhbGc...
Loading dashboard for role: customer
✅ Dashboard loaded: {totalSpent: 0, creditBalance: 0, ...}
✅ Profile loaded: {firstName: "John", lastName: "Doe", ...}
```

### Step 6: Verify Dashboard Shows
You should see:
- ✅ Dashboard card with metrics
- ✅ No redirect back to login
- ✅ Navigation menu visible
- ✅ Profile, Appointments, Parts links work

## Debugging Commands

### Check API Health
```bash
curl http://localhost:5000/api/health
# Response: {"status":"Healthy","timestamp":"2024-..."}
```

### Check Database Connection
```bash
curl http://localhost:5000/api/diagnostics/db-status
# Response: {"database":"Connected","userCount":1}
```

### Check All Users
```bash
curl http://localhost:5000/api/diagnostics/users
# Response: [{"userId":1,"email":"admin@...","role":"admin",...}]
```

### Login and Get Token
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@garagehub.com","password":"Admin@123456"}'

# Response: {"success":true,"token":"eyJhbGc...","role":"admin",...}
```

### Test Protected Endpoint with Token
```bash
curl http://localhost:5000/api/customer/profile \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# Should return profile data if token is valid
```

## Common Issues & Solutions

### Issue: Still Getting 401 After Login
**Solution**:
1. Check browser console for error messages
2. Verify API is running: `curl http://localhost:5000/api/health`
3. Check Visual Studio output for token validation errors
4. Clear browser localStorage: `localStorage.clear()` in console
5. Refresh page and login again

### Issue: "Token Expired" Message Immediately After Login
**Solution**:
1. Check `Jwt:ExpireinMinutes` in `appsettings.json` (should be 60 or higher)
2. Check server time matches client time
3. Verify JWT key hasn't changed: `appsettings.json` Jwt:Key
4. Check authentication logs in Visual Studio output

### Issue: "Unauthorized - Role Check Failed"
**Solution**:
1. Verify user has correct role in database:
   ```sql
   SELECT * FROM users WHERE email = 'your@email.com';
   -- Check the "role" column
   ```
2. Make sure role is exactly "customer", "staff", or "admin" (lowercase)
3. Clear token and login again
4. Check JWT payload at jwt.io to see actual role claim

### Issue: Dashboard Loads But Data is Empty
**Solution**:
1. This is OK - means API is working but methods need to be implemented
2. Check if CustomerService methods exist and are working
3. Look for errors in Visual Studio output
4. This is a data issue, not an authentication issue

### Issue: Parts/Appointments Show "Error Loading"
**Solution**:
1. Check if controller requires admin role but you're logged in as customer
2. PartsController has `[Authorize(Roles = "admin")]` - only admins can see parts
3. AppointmentController allows customers to book/view own appointments
4. Staff can see all appointments with `[Authorize(Roles = "admin,staff")]`

## Browser Console Commands for Debugging

```javascript
// Check stored credentials
localStorage.getItem('authToken')
localStorage.getItem('userRole')

// Check API base URL
getApiBase()

// Test manual API call
await ApiService.get('/customer/profile')

// Check what's in localStorage
JSON.parse(localStorage.getItem('authToken') || '{}')

// Clear everything and reload
localStorage.clear()
location.reload()

// Check current user role from token
const payload = JSON.parse(atob(authToken.split('.')[1]))
console.log(payload)
```

## JWT Token Structure

Your token has this structure:
```javascript
{
  "header": {
    "alg": "HS256",
    "typ": "JWT"
  },
  "payload": {
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "1",
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "admin@garagehub.com",
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "admin",
    "role": "admin",
    "iat": 1234567890,
    "exp": 1234571490
  },
  "signature": "..."
}
```

The "role" claim is what ASP.NET looks for in `[Authorize(Roles = "...")]`.

## Performance Tips

1. **Reduce API Calls**: Don't call API on every page load if data hasn't changed
2. **Cache User Data**: Store profile info in localStorage after first load
3. **Lazy Load**: Load parts/appointments only when user clicks the link
4. **Error Retry**: Implement exponential backoff for failed API calls

## Next Steps

If you're still experiencing issues:

1. ✅ Clear browser cache and cookies
2. ✅ Restart the API server
3. ✅ Check Visual Studio Output for errors
4. ✅ Try incognito/private browser mode
5. ✅ Check PostgreSQL is running and accessible

The login should now work smoothly without any redirect flashing!
