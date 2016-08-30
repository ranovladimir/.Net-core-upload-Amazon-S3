Hi,

This is the back-end part of a sample Angular2/ASP .NET CORE project that upload to Amazon S3 directly from client side.

How it's work : The upload form ask for an authorization to upload in your Amazon's bucket.
 Thanks to the signature and a policy delivered by the back-end, the file to upload is uploads in your Amazon bucket
 without make a first upload in your server side.

 CAUTION : The data forms in the UserCreateComponent have to be the same in the AwsController in the back-end.

To GET STARTED in the .NET CORE App :

Just Fill the fields in the controller.

Run the server

Run your angular2 app

Do not forget to go to ttp://localhost:3000/admin/user/new


Hope that help ! 






***************
*RANO Vladimir*
***************