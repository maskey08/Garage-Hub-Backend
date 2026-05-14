# Code Flow Documentation: Customer Registration

## User Interaction Flow

### 1. Frontend - User Click
```typescript
// File: StaffDashboard.tsx
<Button
  variant="secondary"
  fullWidth
  icon="person_add"
  iconPosition="left"
  onClick={() => setIsAddCustomerModalOpen(true)}
>
  Register Customer
</Button>
```

### 2. Modal Opens
```typescript
// File: StaffDashboard.tsx
<AddCustomerModal
  isOpen={isAddCustomerModalOpen}
  onClose={() => setIsAddCustomerModalOpen(false)}
  onSuccess={() => {
    // Refresh data if needed
  }}
/>
```

### 3. User Fills Form
```typescript
// File: AddCustomerModal.tsx
const [formData, setFormData] = useState({
  firstName: "",
  lastName: "",
  email: "",
  phone: "",
  address: "",
});
```

Form Fields Rendered:
```jsx
<Input label="First Name *" name="firstName" ... />
<Input label="Last Name" name="lastName" ... />
<Input label="Email *" name="email" type="email" ... />
<Input label="Phone *" name="phone" type="tel" ... />
<Input label="Address" name="address" ... />
```

### 4. Form Submission
```typescript
// File: AddCustomerModal.tsx
const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();

  // Validation
  if (!formData.firstName.trim() || !formData.email.trim() || !formData.phone.trim()) {
    setError("First Name, Email, and Phone are required");
    return;
  }

  // API Call
  await staffService.addCustomer({
    firstName: formData.firstName.trim(),
    lastName: formData.lastName.trim(),
    email: formData.email.trim(),
    phone: formData.phone.trim(),
    address: formData.address.trim(),
  });
};
```

### 5. Frontend API Call
```typescript
// File: staffService.ts
addCustomer: (data: AddCustomerRequest) =>
  api.post<Customer>("/staff/customers", data),
```

Request Body:
```json
{
  "firstName": "Arthur",
  "lastName": "Morgan",
  "email": "arthur@example.com",
  "phone": "+1 (555) 000-0000",
  "address": "123 Main Street"
}
```

## Backend Processing

### 1. Controller Receives Request
```csharp
// File: StaffCustomersController.cs
[HttpPost]
public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
{
    // Validation
    if (string.IsNullOrWhiteSpace(dto.FirstName) || 
        string.IsNullOrWhiteSpace(dto.Email) || 
        string.IsNullOrWhiteSpace(dto.Phone))
    {
        return BadRequest(new { message = "First Name, Email, and Phone are required" });
    }

    // Service Call
    var result = await _customerService.CreateCustomerAsync(dto);
    return Ok(result);
}
```

### 2. Service Processing
```csharp
// File: CustomerService.cs
public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto)
{
    // Step 1: Check if email exists
    var existingUser = await _userManager.FindByEmailAsync(dto.Email);
    if (existingUser != null)
        throw new InvalidOperationException("Email already registered");

    // Step 2: Create User object
    var user = new User
    {
        Email = dto.Email,
        UserName = dto.Email,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Phone = dto.Phone,
        EmailConfirmed = true,
        CreatedAt = DateTime.UtcNow
    };

    // Step 3: Generate temporary password
    var tempPassword = GenerateTemporaryPassword();

    // Step 4: Create account
    var result = await _userManager.CreateAsync(user, tempPassword);
    if (!result.Succeeded)
    {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new InvalidOperationException($"Failed to create customer account: {errors}");
    }

    // Step 5: Assign role
    await _userManager.AddToRoleAsync(user, "customer");

    // Step 6: Return DTO
    return new CustomerDto
    {
        Id = user.Id,
        FullName = $"{user.FirstName} {user.LastName}".Trim(),
        Phone = user.Phone,
        Email = user.Email ?? string.Empty,
        Address = dto.Address ?? string.Empty,
        RegisteredDate = user.CreatedAt,
        CreditBalance = 0,
        Vehicles = new(),
        Purchases = new()
    };
}
```

### 3. Database Operations
```
1. UserManager.FindByEmailAsync() → Check Users table
2. UserManager.CreateAsync() → INSERT into Users table
3. UserManager.AddToRoleAsync() → INSERT into UserRoles table
```

## Response Flow

### 1. Backend Response
```json
{
  "id": 5,
  "fullName": "Arthur Morgan",
  "phone": "+1 (555) 000-0000",
  "email": "arthur@example.com",
  "address": "123 Main Street",
  "registeredDate": "2026-04-30T10:30:00Z",
  "creditBalance": 0,
  "vehicles": [],
  "purchases": []
}
```

### 2. Frontend Handles Response
```typescript
// File: AddCustomerModal.tsx
try {
  await staffService.addCustomer({...});

  // Reset form
  setFormData({...});

  // Notify parent
  onSuccess?.();

  // Close modal
  onClose();
} catch (err) {
  setError(err instanceof Error ? err.message : "Failed to add customer");
}
```

### 3. Modal Closes
- User sees success
- Form is reset
- Modal disappears
- Parent component callback is invoked

## Error Handling Paths

### Path 1: Email Already Exists
```
Frontend Input → Backend Validation → InvalidOperationException
→ Controller catches → 409 Conflict Response
→ Frontend catches error → Display error message
```

Error Response:
```json
HTTP 409 Conflict
{
  "message": "Email already registered"
}
```

### Path 2: Missing Required Fields
```
Frontend Validation → Shows error
OR
Backend Validation → 400 Bad Request Response
```

Error Response:
```json
HTTP 400 Bad Request
{
  "message": "First Name, Email, and Phone are required"
}
```

### Path 3: Invalid User Creation
```
Backend UserManager.CreateAsync() fails
→ Collect error messages
→ InvalidOperationException with details
→ 400 Bad Request Response
```

## API Endpoint Summary

### Endpoint
```
POST /api/staff/customers
```

### Authorization
```csharp
[Authorize(Roles = "staff,admin")]
```

### Request
```
Content-Type: application/json
Authorization: Bearer {jwt-token}

{
  "firstName": "string (required)",
  "lastName": "string (optional)",
  "email": "string (required)",
  "phone": "string (required)",
  "address": "string (optional)"
}
```

### Responses

#### Success (200 OK)
```json
{
  "id": 5,
  "fullName": "Arthur Morgan",
  "phone": "+1 (555) 000-0000",
  "email": "arthur@example.com",
  "address": "123 Main Street",
  "registeredDate": "2026-04-30T10:30:00Z",
  "creditBalance": 0,
  "vehicles": [],
  "purchases": []
}
```

#### Conflict (409)
```json
{
  "message": "Email already registered"
}
```

#### Bad Request (400)
```json
{
  "message": "First Name, Email, and Phone are required"
}
```

## Data Persistence

### Database Schema
```sql
-- Users table (AspNetUsers)
INSERT INTO AspNetUsers (
  Email, 
  UserName, 
  FirstName, 
  LastName, 
  Phone, 
  EmailConfirmed, 
  CreatedAt
) VALUES (
  'arthur@example.com',
  'arthur@example.com',
  'Arthur',
  'Morgan',
  '+1 (555) 000-0000',
  1,
  '2026-04-30T10:30:00Z'
);

-- UserRoles table (AspNetUserRoles)
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (5, {customer-role-id});
```

## Security Verification

### Authentication Check
```csharp
[Authorize] // Requires valid JWT token
```

### Authorization Check
```csharp
[Authorize(Roles = "staff,admin")] // Only these roles allowed
```

### Input Validation
```csharp
// Email format validation (via UserManager)
// Password strength validation (via UserManager)
// Required field validation (both frontend and backend)
```

## Complete User Journey Map

```
┌─────────────────────────────────────────────────────────────────┐
│                    Staff Dashboard                              │
└─────────────────────────────────────────────────────────────────┘
                              ↓
                    Click "Register Customer"
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                    Modal Opens                                  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ First Name: [_________]                                 │  │
│  │ Last Name:  [_________]                                 │  │
│  │ Email:      [_________]                                 │  │
│  │ Phone:      [_________]                                 │  │
│  │ Address:    [_________]                                 │  │
│  │                                                          │  │
│  │  [Cancel]                        [Add Customer]         │  │
│  └──────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              ↓
                       Fill Form & Submit
                              ↓
                   Frontend Validation ✓
                              ↓
              POST /api/staff/customers
                    (JWT in header)
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                    Backend Processing                           │
│  1. Validate authorization (staff role)                         │
│  2. Validate input (required fields)                            │
│  3. Check email uniqueness                                      │
│  4. Generate secure password                                    │
│  5. Create User account                                         │
│  6. Assign customer role                                        │
│  7. Save to database                                            │
│  8. Return CustomerDto                                          │
└─────────────────────────────────────────────────────────────────┘
                              ↓
                       Response 200 OK
                      (with customer data)
                              ↓
                    Modal Closes & Resets
                              ↓
                    Parent Callback Called
                              ↓
                    Success Complete ✓
```
