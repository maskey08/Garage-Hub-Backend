# Garage Hub API Testing & Frontend Integration Guide

## 🚀 Quick Start

### 1. Clean Database (Remove Duplicate Tables)
```sql
-- Run this in your PostgreSQL database first
\i cleanup_duplicate_tables.sql
```

### 2. Start the API
```bash
cd GarageHub.API
dotnet run
```

The API will be available at: `https://localhost:7000` or `http://localhost:5000`

## 🔐 Authentication Flow

### Step 1: Register Admin/Staff User
```bash
curl -X POST "https://localhost:7000/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@garagehub.com",
    "password": "Admin@123456",
    "firstName": "Admin",
    "lastName": "User",
    "phone": "+1234567890",
    "role": "admin"
  }'
```

### Step 2: Login to Get Token
```bash
curl -X POST "https://localhost:7000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@garagehub.com",
    "password": "Admin@123456"
  }'
```

**Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "role": "admin",
  "fullName": "Admin User"
}
```

### Step 3: Use Token in Subsequent Requests
```bash
# Method 1: Authorization Header (Recommended)
curl -X GET "https://localhost:7000/api/dashboard/statistics" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# Method 2: Cookie (Alternative)
curl -X GET "https://localhost:7000/api/dashboard/statistics" \
  -H "Cookie: AuthToken=YOUR_TOKEN_HERE"
```

## 📋 Available Endpoints

### 🔓 Public Endpoints (No Auth Required)
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `GET /api/health` - Health check
- `GET /api/info` - API information
- `GET /api/diagnostics/db-status` - Database status
- `GET /api/diagnostics/users` - List all users (for debugging)

### 👤 Customer Endpoints (Role: customer)
- `GET /api/customer/dashboard` - Customer dashboard
- `GET /api/customer/profile` - Get profile
- `PUT /api/customer/profile` - Update profile
- `GET /api/customer/vehicles` - Get customer vehicles
- `POST /api/customer/vehicles` - Add new vehicle
- `GET /api/customer/purchase-history` - Purchase history
- `GET /api/customer/service-history` - Service history

### 📅 Appointment Endpoints (Role: customer)
- `POST /api/appointment` - Book appointment
- `GET /api/appointment` - Get customer appointments
- `PATCH /api/appointment/{id}/cancel` - Cancel appointment

### 👥 Staff Management (Role: admin, staff)
- `GET /api/staff` - Get all staff
- `GET /api/staff/search?searchTerm=john` - Search staff
- `POST /api/staff` - Add staff (admin only)
- `PUT /api/staff/{id}` - Update staff (admin only)
- `DELETE /api/staff/{id}` - Delete staff (admin only)

### 📊 Dashboard & Reports (Role: admin)
- `GET /api/dashboard/statistics` - Dashboard statistics
- `GET /api/reports/daily?date=2026-05-19` - Daily report
- `GET /api/reports/monthly?year=2026&month=5` - Monthly report
- `GET /api/reports/yearly?year=2026` - Yearly report
- `GET /api/reports/low-stock?threshold=10` - Low stock alerts
- `GET /api/reports/top-customers?limit=10` - Top customers

### 🛒 Sales & Parts (Role: staff, admin)
- `POST /api/sales` - Create sale
- `GET /api/sales/users/{id}/loyalty` - Get loyalty points
- `POST /api/sales/send-invoice-email/{saleId}` - Send invoice email
- `GET /api/parts` - Get all parts
- `POST /api/parts` - Add new part
- `PUT /api/parts/{id}` - Update part
- `DELETE /api/parts/{id}` - Delete part

## 🌐 Frontend Integration

### JavaScript/TypeScript Example

```javascript
class GarageHubAPI {
  constructor(baseUrl = 'https://localhost:7000') {
    this.baseUrl = baseUrl;
    this.token = localStorage.getItem('authToken');
  }

  // Set token after login
  setToken(token) {
    this.token = token;
    localStorage.setItem('authToken', token);
  }

  // Make authenticated request
  async request(endpoint, options = {}) {
    const url = `${this.baseUrl}${endpoint}`;
    const config = {
      headers: {
        'Content-Type': 'application/json',
        ...(this.token && { 'Authorization': `Bearer ${this.token}` }),
        ...options.headers
      },
      ...options
    };

    const response = await fetch(url, config);
    
    if (response.status === 401) {
      // Token expired or invalid
      this.token = null;
      localStorage.removeItem('authToken');
      throw new Error('Authentication required');
    }

    return response.json();
  }

  // Auth methods
  async login(email, password) {
    const response = await this.request('/api/auth/login', {
      method: 'POST',
      body: JSON.stringify({ email, password })
    });
    
    if (response.success) {
      this.setToken(response.token);
    }
    
    return response;
  }

  async register(userData) {
    return this.request('/api/auth/register', {
      method: 'POST',
      body: JSON.stringify(userData)
    });
  }

  // Customer methods
  async getCustomerDashboard() {
    return this.request('/api/customer/dashboard');
  }

  async getCustomerVehicles() {
    return this.request('/api/customer/vehicles');
  }

  async addVehicle(vehicleData) {
    return this.request('/api/customer/vehicles', {
      method: 'POST',
      body: JSON.stringify(vehicleData)
    });
  }

  // Admin methods
  async getDashboardStats() {
    return this.request('/api/dashboard/statistics');
  }

  async getStaff() {
    return this.request('/api/staff');
  }

  async getDailyReport(date) {
    return this.request(`/api/reports/daily?date=${date}`);
  }
}

// Usage example
const api = new GarageHubAPI();

// Login
api.login('admin@garagehub.com', 'Admin@123456')
  .then(response => {
    if (response.success) {
      console.log('Logged in successfully');
      // Now you can make authenticated requests
      return api.getDashboardStats();
    }
  })
  .then(stats => {
    console.log('Dashboard stats:', stats);
  })
  .catch(error => {
    console.error('Error:', error);
  });
```

### React Hook Example

```jsx
import { useState, useEffect } from 'react';

export function useGarageHubAPI() {
  const [token, setToken] = useState(localStorage.getItem('authToken'));
  const [user, setUser] = useState(null);
  const baseUrl = 'https://localhost:7000';

  const request = async (endpoint, options = {}) => {
    const url = `${baseUrl}${endpoint}`;
    const config = {
      headers: {
        'Content-Type': 'application/json',
        ...(token && { 'Authorization': `Bearer ${token}` }),
        ...options.headers
      },
      ...options
    };

    const response = await fetch(url, config);
    
    if (response.status === 401) {
      setToken(null);
      setUser(null);
      localStorage.removeItem('authToken');
      throw new Error('Authentication required');
    }

    return response.json();
  };

  const login = async (email, password) => {
    const response = await request('/api/auth/login', {
      method: 'POST',
      body: JSON.stringify({ email, password })
    });
    
    if (response.success) {
      setToken(response.token);
      setUser({ role: response.role, name: response.fullName });
      localStorage.setItem('authToken', response.token);
    }
    
    return response;
  };

  const logout = () => {
    setToken(null);
    setUser(null);
    localStorage.removeItem('authToken');
  };

  return {
    token,
    user,
    login,
    logout,
    request,
    isAuthenticated: !!token
  };
}

// Component example
function Dashboard() {
  const { request, user, isAuthenticated } = useGarageHubAPI();
  const [stats, setStats] = useState(null);

  useEffect(() => {
    if (isAuthenticated && user?.role === 'admin') {
      request('/api/dashboard/statistics')
        .then(setStats)
        .catch(console.error);
    }
  }, [isAuthenticated, user]);

  if (!isAuthenticated) {
    return <LoginForm />;
  }

  return (
    <div>
      <h1>Welcome, {user.name}</h1>
      {stats && (
        <div>
          <p>Total Users: {stats.totalUsers}</p>
          <p>Total Appointments: {stats.totalAppointments}</p>
          {/* ... more stats */}
        </div>
      )}
    </div>
  );
}
```

## 🔧 Common Issues & Solutions

### 401 Unauthorized Errors
1. **Missing Token**: Ensure you're sending the Authorization header
2. **Expired Token**: Tokens expire after 60 minutes, login again
3. **Wrong Role**: Check if your user has the required role for the endpoint
4. **Invalid Token Format**: Use `Bearer YOUR_TOKEN` format

### 404 Not Found Errors
1. **Wrong URL**: Check the endpoint URL matches the documentation
2. **Missing Route**: Ensure the controller action exists
3. **Case Sensitivity**: Use lowercase for routes (e.g., `/api/customer` not `/api/Customer`)

### CORS Issues (Frontend)
The API is configured to allow all origins in development. If you still get CORS errors:
1. Make sure you're using HTTPS for localhost:7000
2. Check browser console for specific CORS error messages
3. Ensure your frontend is making requests to the correct port

### Database Connection Issues
1. Run the cleanup script to remove duplicate tables
2. Check PostgreSQL is running on port 5432
3. Verify database credentials in appsettings.json
4. Check the diagnostic endpoint: `GET /api/diagnostics/db-status`

## 🎯 Testing Checklist

- [ ] Database cleanup completed
- [ ] API starts without errors
- [ ] Can register new users
- [ ] Can login and receive token
- [ ] Can access protected endpoints with token
- [ ] Role-based access works correctly
- [ ] Frontend can authenticate and make API calls
- [ ] All CRUD operations work for each entity
- [ ] Error handling works properly

## 📝 Sample Data for Testing

### Register Customer
```json
{
  "email": "customer@test.com",
  "password": "Customer@123",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890",
  "role": "customer",
  "vehicleNumber": "ABC123",
  "vehicleMake": "Toyota",
  "vehicleModel": "Camry",
  "vehicleYear": 2020
}
```

### Add Vehicle
```json
{
  "vehicleNumber": "XYZ789",
  "make": "Honda",
  "model": "Civic",
  "year": 2021,
  "color": "Blue"
}
```

### Book Appointment
```json
{
  "vehicleId": 1,
  "serviceType": "Oil Change",
  "appointmentDate": "2026-05-20T10:00:00Z",
  "description": "Regular oil change service"
}
```

### Create Sale
```json
{
  "customerId": 1,
  "items": [
    {
      "partId": 1,
      "quantity": 2,
      "unitPrice": 25.99
    }
  ],
  "paymentMethod": "Cash"
}
```