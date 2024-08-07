# Tibber Developer Test - Tomasz Giziński

Hey there ! here is my solution for the cleaning robot task.

I've decided to go with the clean infrastructure and play a bit with minimal api's since I am usually used to the controller approach.

I like to keep the entry points (my controllers) as thin and dumb as possible, therefore I've implemented a very simple CQRS to separate the logic into handlers.

# How to run

Navigate to TibberDeveloperTest/local folder and run the docker compose command.

````docker-compose up -d````

After it's complete, navigate to the API and fire away !
[API swagger](http://localhost:5000/swagger/index.html)

The docker-compose will download and spawn the latest postgres image, as well as restore and build the dockerfile for the API.
The yaml file also includes the environment variable with the postgres connection string.
~~~~
Postgres details:
````
POSTGRES_USER: admin1

POSTGRES_PASSWORD: password2

POSTGRES_DB: tibbertest
````