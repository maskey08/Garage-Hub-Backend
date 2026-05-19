# 🚀 Quick Start - Test the Fixed System

## Step 1: Database Migration (First Time Only)
```bash
cd C:\Users\Anshu\source\repos\AD\Garage-Hub-Backend\GarageHub.API
dotnet ef database update
```

**Expected Output**:
```
✅ Database migrations applied
✅ Admin created successfully
✅ Admin role fixed
```

## Step 2: Start the API
```bash
dotnet run
```

**Expected Output**:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
      Now listening on: https://localhost:7000
```

## Step 3: Open Browser
Navigate to:
```
http://localhost:5000
```

## Step 4: Test Login (No Redirect!)

### Method A: Admin Account
```
Email: smartsikchya.noreply@gmail.com
Password: Admin@123456
```

### Method B: New Customer Account
```
First Name: John
Last Name: Doe
Email: john@example.com
Phone: 9876543210
Password: Password@123
Role: customer
```

## Step 5: Verify Dashboard Loads
✅ You should see:
- Dashboard with metrics cards
- Navigation menu at top
- Profile, Appointments, Parts links
- NO redirect back to login page
- NO token expiration message

## Step 6: Test Features

### Test Profile
1. Click "Profile" in navbar
2. See your information loaded
3. Click "Update Profile" button
4. Edit any field
5. Click "Update Profile"
6. Should see ✅ success message

### Test Appointments
1. Click "Appointments" in navbar
2. Click "Book New Appointment" button
3. Fill form:
   - Vehicle: select from dropdown
   - Date: pick future date
   - Time: pick time
   - Service Type: Oil Change
   - Notes: any text
4. Click "Book Appointment"
5. Should see appointment in list

### Test Logout
1. Click "Logout" button
2. Should return to login page
3. Login again to verify token reset

## Console Debugging (F12)

### Check Network Requests
1. Open DevTools (F12)
2. Go to Network tab
3. Filter by XHR
4. Login and watch requests
5. Should see:
   - `POST /api/auth/login` → 200
   - `GET /api/customer/dashboard` → 200
   - `GET /api/customer/profile` → 200

### Check Browser Storage
1. Application tab → LocalStorage
2. Should see:
   - `authToken` - Long JWT token
   - `userRole` - "customer", "admin", etc.

### Check Console Logs
1. Console tab
2. Should see messages like:
   ```
   ✅ Login successful! Token stored: eyJhbGc...
   Loading dashboard for role: customer
   ✅ Dashboard loaded: {totalSpent: 0, ...}
   ```

## API Health Checks

### In Browser Console:
```javascript
await DiagnosticsApi.health()
// Response: {status: "Healthy", timestamp: "..."}

await DiagnosticsApi.getDbStatus()
// Response: {database: "Connected", userCount: 1}

await DiagnosticsApi.getUsers()
// Response: [{userId: 1, email: "admin@...", role: "admin", ...}]
```

### Using curl:
```bash
curl http://localhost:5000/api/health
curl http://localhost:5000/api/diagnostics/db-status
curl http://localhost:5000/api/diagnostics/users
```

## Common Test Scenarios

### Scenario 1: Fresh Browser
1. Open new Incognito/Private window
2. Go to `http://localhost:5000`
3. Should see login page (not cached state)
4. Register new account
5. Dashboard should load immediately

### Scenario 2: Multiple Logins
1. Login as admin
2. View dashboard
3. Click Logout
4. Login as customer (new account)
5. Should see customer dashboard (not admin)
6. Verify no data from admin account is visible

### Scenario 3: Token Expiration
1. Login and get token
2. Wait 60 minutes (or modify Jwt:ExpireinMinutes to 1)
3. Try to access API
4. Should see "Token expired" message
5. Can login again to get new token

### Scenario 4: Invalid Credentials
1. Try to login with wrong email
2. Should see error: "Invalid credentials"
3. Try to login with wrong password
4. Should see error: "Invalid credentials"
5. Should NOT redirect to dashboard

## Expected Console Output (Visual Studio)

```
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/1.1 POST http://localhost:5000/api/auth/login
info: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core initialized
✅ Database migrations applied
✅ Admin exists (Id: 1) — all good
🔑 Token received for /api/auth/login
✅ Token valid - UserId: 1, Role: admin
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executed endpoint 'GarageHub.API.Controllers.AuthController.Login (GarageHub.API)'
```

## If Something Goes Wrong

### Login Still Redirects?
```javascript
// In console, check what's happening:
localStorage.clear()
location.reload()
// Try logging in again
```

### 401 Errors?
```bash
# Check API is running
curl http://localhost:5000/api/health

# Check database
curl http://localhost:5000/api/diagnostics/db-status

# Check users exist
curl http://localhost:5000/api/diagnostics/users
```

### 404 Errors?
```javascript
// Check API URL in console
getApiBase()
// Should return correct URL

// Try direct API call
await ApiService.get('/customer/profile')
// Should return profile or 401 (not 404)
```

### Dashboard Data Missing?
```javascript
// Check what's loaded
await CustomerApi.getProfile()
await CustomerApi.getDashboard()

// If returns error, check role:
localStorage.getItem('userRole')
// Should be "customer" or "admin"
```

## Success Checklist ✅

- [ ] API starts without errors
- [ ] Frontend loads at http://localhost:5000
- [ ] Login page appears
- [ ] Can login with admin account
- [ ] Dashboard shows after login (within 2 seconds)
- [ ] NO redirect back to login page
- [ ] Navbar shows (Profile, Appointments, Parts, Logout)
- [ ] Can click on different sections
- [ ] Appointments can be booked
- [ ] Profile can be updated
- [ ] Logout works
- [ ] Can login again after logout
- [ ] Browser console has no red errors
- [ ] Visual Studio output shows successful connections
- [ ] Database shows connected

## Performance Metrics

- **Login to Dashboard**: < 2 seconds
- **Profile Load**: < 1 second
- **Appointments Load**: < 1 second
- **Parts Load**: < 1 second

## Next Steps After Verification

1. ✅ Test all features work
2. ✅ Review error handling in edge cases
3. ✅ Test with multiple users simultaneously
4. ✅ Test role-based access (admin vs customer)
5. ✅ Test refresh/back button behavior
6. ✅ Test on different browsers
7. ✅ Prepare for production deployment

---

**Total Setup Time**: ~5 minutes
**Testing Time**: ~10 minutes
**Status**: Ready to use! 🎉
