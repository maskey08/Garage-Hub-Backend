# Testing Guide for Bug Fixes

## Test 1: Password Field Spacing on Login Page

### Steps:
1. Navigate to the login page
2. Look at the password field section
3. Verify "Forgot Password?" link has proper spacing

### Expected Result:
- ✅ "Password" label and "Forgot Password?" link are on the same line
- ✅ There is clear vertical spacing (at least 12px margin) before the password input field
- ✅ The input field has proper padding and doesn't overlap with the label

---

## Test 2: Session Logout and Cookie Cleanup

### Setup:
1. Start both frontend and backend servers
2. Log in with any valid credentials

### Steps:
1. **Verify Login Success**:
   - [ ] You're redirected to the dashboard
   - [ ] User name appears in the top bar
   - [ ] Sidebar displays navigation items

2. **Initiate Logout**:
   - [ ] Click the "Logout" button in the sidebar
   - [ ] Loading state shows (text changes to "Logging out...")

3. **Verify Backend Logout**:
   - [ ] Check backend logs for `/auth/logout` endpoint call
   - [ ] Verify response status is 200
   - [ ] Check that cookies are cleared in response headers

4. **Verify Frontend Cleanup**:
   - [ ] Browser redirects to login page
   - [ ] User name in top bar disappears
   - [ ] localStorage is cleared (check via DevTools)
     - Open DevTools → Application → Storage → Local Storage
     - Verify `token`, `role`, `fullName` are removed
   - [ ] sessionStorage is cleared
     - Open DevTools → Application → Storage → Session Storage
     - Should be empty

5. **Verify Session is Destroyed**:
   - [ ] Manually navigate to `/staff/dashboard` using URL
   - [ ] You're redirected to login page (protected route)
   - [ ] Previous session data is not accessible

### Expected Results:
- ✅ Logout completes successfully
- ✅ All auth data is cleared
- ✅ Cookies are deleted
- ✅ Session is completely invalidated
- ✅ Cannot access protected routes without re-logging in

---

## Test 3: Error Handling During Logout

### Steps:
1. Log in successfully
2. Open DevTools → Network tab
3. Simulate network failure:
   - Check "Offline" in DevTools or throttle network
4. Click Logout button

### Expected Results:
- ✅ Even if API call fails, local storage is cleared
- ✅ User is redirected to login page
- ✅ No critical errors in console
- ✅ App remains functional

---

## Test 4: Logout in Different Browsers/Tabs

### Steps:
1. Open your app in two browser tabs (same browser)
2. Log in in Tab 1
3. Click logout in Tab 1
4. Switch to Tab 2

### Expected Results:
- ✅ Tab 1 shows login page
- ✅ Tab 2 still shows dashboard (not automatically synced)
  - Note: This is expected - LocalStorage changes aren't automatically synced across tabs
  - When you try to navigate in Tab 2, the app will detect no token and redirect to login

### Alternative Test (Advanced):
1. Use private/incognito windows for each tab
2. Same steps as above
3. Each tab maintains its own storage

---

## Test 5: AddCustomerModal Button Click

### Steps:
1. Navigate to Staff Dashboard
2. Click "Register Customer" button in Quick Actions
3. Fill in the form:
   - First Name: John
   - Email: john@example.com
   - Phone: 555-1234
4. Click "Add Customer" button

### Expected Results:
- ✅ Modal appears without errors
- ✅ Form validates correctly
- ✅ Button click doesn't throw TypeScript errors
- ✅ Submission works (either succeeds or fails with proper error message)
- ✅ No console errors about type mismatches

---

## Test 6: TypeScript/ESLint Validation

### Steps:
1. Run `npm run build` in frontend directory
2. Run `dotnet build` in backend directory

### Expected Results:
- ✅ No TypeScript errors
- ✅ No ESLint errors (or only non-blocking warnings)
- ✅ Build completes successfully
- ✅ No `no-explicit-any` warnings in auth files
- ✅ No `react-refresh` warnings

---

## Test 7: Login Flow with Error Handling

### Steps:
1. Navigate to login page
2. Try logging in with invalid credentials

### Expected Results:
- ✅ Error message displays properly
- ✅ No type-related errors in console
- ✅ Error is clearly visible to user
- ✅ Can retry login

### Steps:
3. Log in with valid credentials

### Expected Results:
- ✅ Authentication succeeds
- ✅ Token is stored in localStorage
- ✅ User is logged in with correct role
- ✅ Redirected to correct dashboard

---

## Test 8: Logout After Network Recovery

### Steps:
1. Simulate offline mode (DevTools)
2. Click logout (should fail API call but still clear local data)
3. Disable offline mode (network recovered)
4. Try to access protected route

### Expected Results:
- ✅ Even though logout API call failed, user is logged out locally
- ✅ Trying to access protected route redirects to login
- ✅ No "stuck" state with invalid token

---

## Test 9: Verify No Password Field Overlap

### Steps:
1. Open login page in different screen sizes:
   - [ ] Desktop (1920x1080)
   - [ ] Tablet (768x1024)
   - [ ] Mobile (375x667)

### Expected Results:
- ✅ All screen sizes show proper spacing
- ✅ No text overlap
- ✅ "Forgot Password?" link is clickable
- ✅ Password input is fully visible

---

## Test 10: Logout Across Multiple Sessions

### Steps:
1. Log in as Admin
2. Click logout
3. Log in as Staff
4. Click logout
5. Log in as Customer
6. Click logout

### Expected Results:
- ✅ Each login/logout cycle works correctly
- ✅ No data from previous sessions persists
- ✅ No errors after multiple login/logout cycles
- ✅ Each role's dashboard loads correctly

---

## Checklist for QA

- [ ] Password field spacing looks good
- [ ] Logout clears all cookies
- [ ] Logout clears localStorage
- [ ] Logout clears sessionStorage
- [ ] Logout redirects to login page
- [ ] Can't access protected routes after logout
- [ ] AddCustomerModal button works
- [ ] No TypeScript errors
- [ ] No ESLint errors
- [ ] Login error handling works
- [ ] Multiple logout cycles work
- [ ] Works on mobile/tablet/desktop
- [ ] Works with network failures

---

## Debug Commands

### Frontend DevTools Console:
```javascript
// Check localStorage
console.log('localStorage:', localStorage);

// Check sessionStorage
console.log('sessionStorage:', sessionStorage);

// Check if token is set
console.log('Token:', localStorage.getItem('token'));

// Check cookies
console.log('Cookies:', document.cookie);
```

### Backend (C#):
```csharp
// Check if user has token claim
var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
var role = User.FindFirstValue(ClaimTypes.Role);
Console.WriteLine($"UserId: {userId}, Role: {role}");
```

---

## Known Limitations

1. LocalStorage changes aren't automatically synced across browser tabs
   - This is browser behavior, not a bug
   - User would need to refresh the page to see logout effect in other tabs

2. JWTs are stateless
   - Logout on backend doesn't actually invalidate the token
   - Only client-side storage is cleared
   - If someone has the token, they could technically use it until expiration
   - Consider implementing a token blacklist for production

3. Private/Incognito windows
   - Each window maintains separate storage
   - Logout in one window doesn't affect others

---

## Troubleshooting

### Issue: "Still logged in after logout"
**Solution**: 
- Check if localStorage was actually cleared
- Check browser cookies
- Verify backend logout endpoint was called

### Issue: "Logout button not responding"
**Solution**:
- Check network tab in DevTools
- Look for errors in console
- Verify authorization header is being sent

### Issue: "Password field looks cramped"
**Solution**:
- Clear browser cache
- Hard refresh (Ctrl+F5 or Cmd+Shift+R)
- Check CSS files were loaded correctly

### Issue: "Build fails with TypeScript errors"
**Solution**:
- Run `npm install` to ensure dependencies are correct
- Check that all type definitions are in place
- Verify no stray `any` types remain

---

## Success Criteria

✅ All tests pass
✅ No console errors
✅ No type warnings
✅ Logout works across all user roles
✅ Session is properly invalidated
✅ No data leaks between sessions
✅ UI looks correct on all screen sizes
✅ Password field has proper spacing
✅ Customer registration modal works

---

## Sign-Off

- [ ] QA approved
- [ ] Testing completed: ___________
- [ ] Date: ___________
- [ ] Tester: ___________
