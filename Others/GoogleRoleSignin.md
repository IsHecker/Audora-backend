Since your frontend is a SPA and the backend is API-only, you can't render a role selection view like in server-side MVC. Instead, follow this flow:

---

### ✅ Clean Flow for SPA + API with External Login (Google)

#### 1. **Frontend initiates Google sign-in**

Use something like Firebase, Google SDK, or OAuth2 popup to get the Google access token or ID token.

#### 2. **Frontend sends token to backend**

Once Google signs the user in, your frontend sends the ID token to your backend (e.g., `POST /auth/external/google`).

#### 3. **Backend validates token and checks if account exists**

In your backend:

* Validate the Google token.
* Extract the email.
* Check if a user with that email exists:

  * ✅ If **yes** → sign them in.
  * ❌ If **no** → respond with `200 OK` and a flag like `{"requiresRoleSelection": true}`.

#### 4. **Frontend shows role selection UI**

If backend says the user doesn’t exist, show a form (e.g., dropdown) to let them pick `Listener` or `Creator`.

#### 5. **Frontend submits selected role to backend**

After the user chooses, frontend sends:

```json
POST /auth/external/register
{
  "idToken": "...",
  "role": "Creator"
}
```

#### 6. **Backend creates user, assigns role, links external login**

Now your backend:

* Validates the token again.
* Creates the new user.
* Assigns the selected role.
* Links the external login provider.
* Signs in and returns JWT.

---

### ✅ Summary

This flow fits SPA + API architecture:

* Google auth handled on the frontend.
* Role selection handled by the frontend **before** backend account creation.
* Backend is stateless and only handles decisions based on tokens and requests.

Let me know if you want a Clean Architecture breakdown of the endpoints or commands involved.