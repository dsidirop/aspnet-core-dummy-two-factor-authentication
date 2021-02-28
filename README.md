# ASPNET CORE Dummy 2 Factor Authentication

Proof of concept website for two factor authentication (2 challenges ala "guess the password").

The site has 3 simple views:

1. The first screen is the landing page which has just a simple password field and a submit ('login') button.
    It also contains a *hidden* input field right before the visible password field, containing the
    password in a ASCII format. When the user submit in the correct password he is redirected to
    the second screen.

2. The second screen has (again) a password input field and a submit ('login') button. It also has a specially
    crafted image containing the correct password embedded into the bottom of the image. When the user types in
    the correct password he's navigated to Page 3.

3. The third screen should include a title congratulating the user on logging in. It should include a download
    button. The download button should download this README.md file.

This is essentially a custom two-factor authentication mechanism. Like with all sane two-factor authentication mechanisms,
provisions have been taken so that users who haven't succeeded in the first password challenge within the last X hours
(where X is configurable and set to 24-hours by default) cannot proceed to step 2 via deep-link. If they attempt to do so
they get redirected back to step 1.
