To pull from DockerHUB

just run on your local docker machine:

docker run -rm -p 5181:80 nunorelvao/frameworkbase

-- to build in context

->from root project sln
 docker build -f .\FrameworkBase\WebFrameworkBase\Dockerfile . -t webdocker:latest