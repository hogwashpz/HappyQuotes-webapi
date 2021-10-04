# HappyQuotes WebAPI

Simple Web API with one endpoint returning random google search image url


## Run Locally

Clone the project

```bash
git clone https://github.com/hogwashpz/HappyQuotes-webapi
```

Check if you have **.NET 6 SDK** installed (6.0.100-rc.1 at time of writing)

```bash
dotnet --list-sdks
```

#### Before building and runnig

From Google Developer dashboard obtain ApiKey and Custom Search EngineID
and set them in **BuildAndRun.bat**

Then in solution root execute

```bash
BuildAndRun.bat
```
 

## API Reference

#### Get random happy quote image url with title

```https
GET /api/images/random
```

#### API documentation

```https
GET /swagger
```
