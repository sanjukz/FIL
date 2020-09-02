## Kz_Repo
**Go check the Github Wiki for the project:-** https://github.com/Kyazoonga/Kz/wiki
# Kz Develop Jenkin build status 
**Build Status    :-**
[![Build Status][![Build Status](http://34.217.65.13:8080/buildStatus/icon?job=Develop)](http://34.217.65.13:8080/job/Develop/)

**Tests Status     :-**
[![Build Status][![Build Status](http://34.217.65.13:8080/buildStatus/icon?job=Develop)](http://34.217.65.13:8080/job/Develop/)

**Publish Status  :-**
[![Build Status][![Build Status](http://34.217.65.13:8080/buildStatus/icon?job=Develop)](http://34.217.65.13:8080/job/Develop/)

# Testing Production Build
Production build involves building the project but also "packing" the front end. It is good to test this if you have made a decent sized change to the front end.  The output folder site-deploy is git ignored.

1. cd into a Kz.Web.* folder
2. Run dotnet publish -o site-deploy -c Release
