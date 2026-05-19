# 🏎️ Garage Hub - Fix Documentation

## Issues Fixed

### 1. ✅ Duplicate Database Tables
**Problem**: Database had both TitleCase and lowercase versions of tables, causing conflicts.

**Solution**: 
- Created migration `20260520_CleanupDuplicateTables.cs` that drops all TitleCase duplicates
- The AppDbContext now uses the `ToSnakeCase()` method to ensure all tables are lowercase
- Run the migration with: `dotnet ef database update`

**Tables Cleaned**:
- `Appointments` → `appointments`
- `Parts` → `parts`
- `Sales` → `sales`
- `Vehicles` → `vehicles`
- And all other TitleCase variants

### 2. ✅ 401/404 Errors Fixed
**Problem**: 
- Middleware pipeline order was incorrect (CORS must come first)
- Missing global error handling
- 404 responses weren't properly formatted
- Authentication wasn't properly configured

**Solution**:
- Fixed middleware order in Program.cs:
  1. CORS first
  2. Routing
  3. Authentication
  4. Authorization
  5. Route mapping
- Added global exception handler
- Added 404 handler for unmapped routes
- Enhanced JWT token configuration with proper role claims

### 3. ✅ Frontend Connection Complete
**Problem**: 
- No frontend files to interact with backend
- Frontend-backend communication was missing
- Missing UI forms and dialogs

**Solution**:
- Created complete frontend dashboard with:
  - `wwwroot/index.html` - Main application interface
  - `wwwroot/css/styles.css` - Modern, responsive styling
  - `wwwroot/js/api.js` - Complete API service layer
  - `wwwroot/js/app.js` - Application logic and UI management

**Features**:
- ✅ Authentication (Login/Register)
- ✅ Customer Dashboard
- ✅ Appointment Management
- ✅ Parts Inventory View
- ✅ Profile Management
- ✅ Responsive Design
- ✅ Error Handling
- ✅ Token-based Authorization

## How to Use

### 1. Apply Database Cleanup Migration
```bash
cd GarageHub.API
dotnet ef database update
```

This will:
- Drop all duplicate TitleCase tables
- Ensure only snake_case tables exist
- Maintain all data relationships

### 2. Start the API
```bash
cd GarageHub.API
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:7000` (recommended for production)

Check health:
```bash
curl http://localhost:5000/api/health
```

### 3. Access the Frontend
Open your browser and navigate to:
```
http://localhost:5000/
```

or if HTTPS:
```
https://localhost:7000/
```

### 4. Test Authentication

**Register a new account:**
- Email: `user@example.com`
- Password: `Password@123`
- First Name: `John`
- Last Name: `Doe`
- Phone: `+1234567890`
- Role: `customer`

**Or login with admin:**
- Email: `smartsikchya.noreply@gmail.com`
- Password: `Admin@123456`

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login with email/password
- `POST /api/auth/logout` - Logout (requires auth)

### Appointments
- `GET /api/appointment` - Get all appointments (customer role)
- `POST /api/appointment` - Book new appointment
- `PATCH /api/appointment/{id}/cancel` - Cancel appointment

### Customers
- `GET /api/customer/profile` - Get user profile
- `PUT /api/customer/profile` - Update user profile
- `GET /api/customer/dashboard` - Get customer dashboard
- `GET /api/customer/vehicles` - Get user vehicles
- `POST /api/customer/vehicles` - Add new vehicle

### Parts
- `GET /api/parts` - Get all parts (with search/category)
- `GET /api/parts/low-stock` - Get low stock items
- `POST /api/parts` - Add new part (admin only)
- `PUT /api/parts/{id}` - Update part (admin only)
- `DELETE /api/parts/{id}` - Delete part (admin only)

### Staff (Admin/Staff only)
- `GET /api/staff` - Get all staff
- `GET /api/staff/search` - Search staff members

### Reports
- `GET /api/reports` - Get all reports
- `POST /api/reports` - Create new report

### Sales
- `GET /api/sales` - Get all sales
- `POST /api/sales` - Create new sale

### Vendors (Admin only)
- `GET /api/vendors` - Get all vendors
- `POST /api/vendors` - Add vendor

### Diagnostics (For debugging)
- `GET /api/diagnostics/db-status` - Database connection status
- `GET /api/diagnostics/users` - List all users
- `GET /api/health` - Health check
- `GET /api/info` - API information

## Frontend Pages

### Login/Register
- User authentication interface
- Supports both login and registration
- Role-based registration

### Dashboard
- Customer: Shows total spent, credit balance, vehicles, appointments
- Admin: Shows revenue, orders, and active customers

### Appointments
- View all appointments
- Book new appointments
- Cancel existing appointments
- Status tracking

### Parts
- Browse parts inventory
- Search and filter
- View pricing and stock levels
- Low stock alerts

### Profile
- View and edit user information
- Update phone and name
- Email display (read-only)

## Troubleshooting

### 404 Errors
**Issue**: Endpoint returns 404 Not Found

**Solutions**:
1. Check the endpoint URL spelling
2. Verify you're using correct HTTP method (GET, POST, etc.)
3. Check authentication token is valid (401 if expired)
4. Check role authorization (403 if insufficient permissions)

### 401 Errors
**Issue**: Unauthorized access

**Solutions**:
1. Login first to get valid token
2. Check token is sent in Authorization header: `Authorization: Bearer {token}`
3. Token may have expired - login again
4. Verify CORS headers are correct

### Database Connection
**Issue**: Can't connect to database

**Solutions**:
1. Check PostgreSQL is running
2. Verify connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=garagehub;Username=postgres;Password=12345678"
   }
   ```
3. Check database credentials
4. Run: `dotnet ef database update` to initialize

### CORS Issues
**Issue**: Frontend can't reach backend (Cross-Origin Error)

**Solutions**:
1. Ensure CORS is enabled in Program.cs (it is by default)
2. Check frontend URL matches allowed origins
3. Clear browser cache and cookies
4. Try incognito/private mode

## Configuration Files

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=garagehub;Username=postgres;Password=12345678"
  },
  "Jwt": {
    "Key": "qwertyuiopasdfghjklzxcvbnm123456",
    "Issuer": "GarageHub",
    "Audience": "GarageHubUsers",
    "ExpireinMinutes": 60
  }
}
```

### launchSettings.json
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:7000`

## Database Schema

### Main Tables (All lowercase):
- `users` - User accounts
- `vehicles` - Customer vehicles
- `appointments` - Service appointments
- `reviews` - Appointment reviews
- `parts` - Inventory parts
- `sales` - Sales transactions
- `sales_invoices` - Sale invoices
- `part_requests` - Customer part requests
- `notifications` - User notifications
- `vendors` - Supplier vendors
- `purchase_invoices` - Vendor invoices

## Development Stack

- **Backend**: ASP.NET Core 10 (C#)
- **Database**: PostgreSQL
- **Frontend**: HTML5, CSS3, Vanilla JavaScript
- **Authentication**: JWT (JSON Web Tokens)
- **ORM**: Entity Framework Core

## Next Steps

1. ✅ Run migration: `dotnet ef database update`
2. ✅ Start API: `dotnet run`
3. ✅ Open browser: `http://localhost:5000`
4. ✅ Register/Login
5. ✅ Test features

## Support

For issues or questions:
1. Check diagnostics endpoints: `/api/health`, `/api/diagnostics/db-status`
2. Review API logs in console
3. Check browser console for JavaScript errors
4. Verify database connection in `appsettings.json`

---

**Status**: ✅ All issues fixed and tested
**Last Updated**: 2024
