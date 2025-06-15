This is a **common and well-defined problem** in authentication systems: supporting both **local auth (email/password)** and **external identity providers (IDPs)** like Google or Facebook, while maintaining **a unified user identity** in your app. Here's how to handle it **cleanly** with ASP.NET Identity:

---

### ✅ **1. Use ASP.NET Identity as the Core User System**

Even if you support multiple sign-in methods, all users — local or external — should be mapped to a single `ApplicationUser` entry in the database. That way, your `Guid`-based User ID stays consistent and references across your domain are preserved.

---

### 🧠 **What Are Those Identity Tables For?**

When you scaffold Identity, you get:

| Table              | Purpose                                                               |
| ------------------ | --------------------------------------------------------------------- |
| `AspNetUsers`      | Your main `ApplicationUser` table. You store user-specific info here. |
| `AspNetUserLogins` | External login mapping table (e.g. Google, Facebook).                 |
| `AspNetUserClaims` | Stores additional claims about the user.                              |
| `AspNetUserRoles`  | Maps users to roles (if you're using role-based auth).                |

So your **local and Google users will both be stored in `AspNetUsers`** — the login method just affects how they get authenticated and created.

---

### ✅ **2. Use `AspNetUserLogins` to Handle External Auth**

This is where Identity shines.

#### When a user logs in with Google:

* You **extract the Google `sub` claim**.
* You check if there's an entry in `AspNetUserLogins` with:

  * `LoginProvider = "Google"`
  * `ProviderKey = <GoogleSub>`
* If it exists → Sign in the linked `ApplicationUser`.
* If it doesn't → Create a new `ApplicationUser`, and add a login:

  ```csharp
  await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", sub, "Google"));
  ```

This keeps your system **decoupled** from whatever IDP is being used.

---

### ✅ **3. How to Keep Using Guid User IDs**

Since you're already using `Guid` as your primary user ID in the domain and app layers:

* Customize your `ApplicationUser` class:

  ```csharp
  public class ApplicationUser : IdentityUser<Guid>
  {
      // additional props
  }
  ```

* Configure Identity to use `Guid`:

  ```csharp
  services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
          .AddEntityFrameworkStores<AppDbContext>()
          .AddDefaultTokenProviders();
  ```

This way, all your users — local or external — will have consistent `Guid` IDs.

---

### ✅ **4. Use `UserManager` + JWT Token System**

You don’t have to use cookie auth. You can keep your JWT token flow:

* After successful login (local or external), **build a JWT** using `ApplicationUser.Id`.
* Inject claims/roles as usual.
* Use `UserManager` and `SignInManager` to help abstract all logic.

---

### ✅ **5. Prevent Duplicate Accounts**

If the same email is used across local and external providers, you have **two options**:

* **Link them manually** if the user verifies ownership of both (not automatic).
* **Keep them separate** (as you mentioned: one provider per user).

To enforce the second approach, **validate that no user exists with the same email before creating an external user**, or disallow local signup if email is taken by an external login.

---

### 🔁 Summary – What You Should Do

1. ✅ Use ASP.NET Identity with `Guid` IDs.
2. ✅ Store all users in `AspNetUsers`.
3. ✅ Use `AspNetUserLogins` to track external provider accounts.
4. ✅ Generate JWTs manually using `ApplicationUser.Id`.
5. ✅ Ensure email uniqueness logic based on your policy (one account per provider or linked accounts).

---

If you want, I can walk you through implementing this cleanly in your architecture. Just ask!