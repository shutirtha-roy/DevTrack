# DevTrack

A web application with ASP.NET Core to track activities from users such as tracked time, 
mouse clicks, screenshots, web camera images, and running 
applications on the desktop. The tracking data is fetched from a 
desktop application using Web API (RESTful). This project was developed as the final project of the ASP.Net Core training course from Dev Skill. 

Users can create new projects and invite others to the project by email. 
The invitation email is sent using Worker Service. Four others and I 
worked collaboratively to develop this project within a month. We have 
used the MSSQL server to store the data, and the project has been 
Dockerized.

Owners or the users can also examine or print their report(data which are fetched from Web API) to see their time tracking details which includes Screenshots, WebCaputures, Keyboard Hits, Mouse Hits, Running Programs and Windows.

## Web API
|  |  |  |  |
|---------|---------|---------|---------|
| <img src="ApplicationScreenshots/API/1.%20API_HomePage.png"> | <img src="ApplicationScreenshots/API/2.%20API_Login.png"> | <img src="ApplicationScreenshots/API/3.%20API_GetProject.png"> | <img src="ApplicationScreenshots/API/4.%20API_POSTProject.png"> |

## Web Application
### Login and Sign Up
|  |  |
|---------|---------|
| <img src="ApplicationScreenshots/WEB/General/1.%20Login.PNG"> | <img src="ApplicationScreenshots/WEB/General/2.%20SignUp.PNG">|

### Dashboard
|  |
|---------|
| <img src="ApplicationScreenshots/WEB/General/3.%20HomePage.PNG"> |

### Project Creation, Editing & Details
|  |  |  |  |
|---------|---------|---------|---------|
| <img src="ApplicationScreenshots/WEB/Project/5.%20CreateProject_GET.PNG"> | <img src="ApplicationScreenshots/WEB/Project/6.%20CreateProject_POST.PNG"> | <img src="ApplicationScreenshots/WEB/Project/1.%20ShowProjects_.PNG"> | <img src="ApplicationScreenshots/WEB/Project/2.%20ShowProjectDetails.PNG"> |
|  |  |  |
| <img src="ApplicationScreenshots/WEB/Project/4.%20ShowProjects.PNG"> | <img src="ApplicationScreenshots/WEB/Project/7.%20EditProject_POST.PNG"> | <img src="ApplicationScreenshots/WEB/Project/3.%20ShowProjectsArchived.PNG"> 

### Report
|  |  |  |  |
|---------|---------|---------|---------|
| <img src="ApplicationScreenshots/WEB/Report/0_SelectReportType.png"|

### Member Invitation & Response
|  |  |
|---------|---------|
| <img src="ApplicationScreenshots/WEB/General/1.%20Login.PNG"> | <img src="ApplicationScreenshots/WEB/General/2.%20SignUp.PNG">|

### Settings
|  |  |
|---------|---------|
| <img src="ApplicationScreenshots/WEB/Settings/1.%20ProfilePic.PNG"> | <img src="ApplicationScreenshots/WEB/Settings/2_MemberViewTimezone.png">|

## License

This project is licensed under the [Apache License 2.0](LICENSE).
