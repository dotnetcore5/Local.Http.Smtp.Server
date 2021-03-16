# Local Http & Smtp Server ![GitHub release](https://img.shields.io/github/release/ajeetx/Local.Http.Smtp.Server.svg?style=for-the-badge) ![Maintenance](https://img.shields.io/maintenance/yes/2021.svg?style=for-the-badge)   

A basic http server to serve http request and,
A simple SMTP server to receive email


## Repository codebase
 
The repository consists of projects as below:


| # |Project Name | Project detail | location| Environment |
| ---| ---  | ---           | ---          | --- |
| 1 | Local.Http.Smtp.Server | Dotnet5 console app  |  **Local.Http.Smtp.Server** folder | [![.Net Framework](https://img.shields.io/badge/DotNet-5.0-blue.svg?style=plastic)](https://dotnet.microsoft.com/download/dotnet/5.0)|
| 2 | Email.Test.App | Azure function to send email |  **Email.Test.App** folder | [![.Net Framework](https://img.shields.io/badge/DotNet-5.0-blue.svg?style=plastic)](https://dotnet.microsoft.com/download/dotnet/5.0)| 

### Summary

The overall objective of the applications :

```

>   A user can browse to "http://localhost:5000/". 
>   The page is served through the **http server**. 
>   The console application logs the browsing.
>   User can send email and see the logs within the console.

```

### Setup detail


> Download/install   	
>	1.	[![.Net Framework](https://img.shields.io/badge/DotNet-5.0-blue.svg?style=plastic)](https://dotnet.microsoft.com/download/dotnet/5.0) to run webapi project
>   
>   2. [![VS2019](https://img.shields.io/badge/VS-2019-blue.svg?style=plastic)](https://visualstudio.microsoft.com/downloads//) to run/debug the applications
>   Or [![VSCode](https://img.shields.io/badge/VS-Code-blue.svg?style=plastic)](https://code.visualstudio.com/) to run/debug the applications
>	
>   


##### Project Setup detail

>   1. Please clone or download the repository from [![github](https://img.shields.io/badge/git-hub-blue.svg?style=plastic)](https://github.com/AJEETX/Local.Http.Smtp.Server) 
>   
>   2. Create a folder and place the downloaded repository and unzip if downloaded.
>   
>   3. Open the solution file through **Visual Studio2019** where the repository is downloaded
>   
##### (a) To start the webapi
   
>   1. Through **Visual Studio2019**, click **F5** button to run the **http server** & **smtp server**, Please make sure to select as **multiple-startup-project**.

*********************************************************************************************

<img src="multiple-projects.PNG" />

*********************************************************************************************
>   
>   3. **http server** shall start running on port **5000**
>
>   4. **smtp server** shall start running on port **25**
>

*********************************************************************************************

<img src="server.PNG" />

*********************************************************************************************
>
>   5. open a browser with url as **http://localhost:5000**
>   
 *********************************************************************************************

<img src="http-post-page.PNG" />

*********************************************************************************************
>
>   5. The http request/response is logged on the console.
>   6. Then email received is logged on the console
>   
 *********************************************************************************************

<img src="email-sent-received.PNG" />

*********************************************************************************************
```
For better experience please use chrome browser
```
-----------------------------------------------------------------------



![Visitor Badge](https://visitor-badge.laobi.icu/badge?page_id=ajeetx/smtpserver)  | ![GitHub contributors](https://img.shields.io/github/contributors/ajeetx/Local.Http.Smtp.Server.svg?style=plastic)|![license](https://img.shields.io/github/license/ajeetx/Local.Http.Smtp.Server.svg?style=plastic)|
 | --- | --- | ---|



