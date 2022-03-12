# Smart-Apartment-Assessment
This is a simple implementation of a search engine in C#.Net for looking up apartment data in selected markets, sitting on AWS ElasticSearch with the aid of third party clients NEST and Elasticsearch.Net.

## Project Architecture
The clean architecture pattern has been adopted for this project for its ability to eliminate dependency of the application layer on the infrastructure and UI layers thereby making it easier to unit test and substitute or scale up these layers.

## Design Pattern
The design pattern adopted for this project is Mediator which uses a mediator object to handle interaction between the UI and the Infrastructure layers.

## Third-party libraries
### 1. MediatR
This is a nuget package that carries out the function of a mediator as required by our design choice

### 2. NEST
A high-level library for building applications on the elasticsearch environment.
