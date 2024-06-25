namespace AngularAuthAPI.Helpers
{
    public static class ForgotPasswordEmailBody
    {

        public static string EmailStringBody(string firstName,string lastname, string url, string applicationName)
        {
            return $@"<html>
<head>
</head>
<body style=""margin:0;padding:0;font-family:Arial,Helvetica,sans-serfi;"">
<div style=""height:auto;background:linear-gradient(to top,#c9c9ff 50%,#6e6ef6 90%) no-repeat;wodth:400px;padding:30px"">
<div>
<div>
<h1>Confirm your email</h1>
<hr>
<p>Hello: {firstName} {lastname} </p>
<p>In order to reset your password, please click on the following link</p>
<a href=""{url}"" target=""_blank"" style=""background:#0d6efd;padding:10px;border:none;
color:white;border-radius:4px;display:block;margin:0 auto;width:50%;text-align:center;text-decoration:none"">Click here</a><br>
<p>Kind Regards,<br><br>
{applicationName} </p>
</div>
</div>
</div>
</body>
</html>

";
        }


    }
}
