# Travel and Accommodation Booking Platform API 🌍✈️

The **TAPB API** provides a suite of endpoints to manage various aspects of hotel operations. It includes features for handling reservations, managing hotels and city data, and delivering services to guests, all designed to enhance the booking experience.

## API Endpoints

### Amenities
- **GET**: `/api/amenities`  
   Retrieve a page of amenities
- **POST**: `/api/amenities`  
   Create a new amenity
- **GET**: `/api/amenities/{id}`  
   Get an amenity specified by ID
- **PUT**: `/api/amenities/{id}`  
   Update an existing amenity

### Auth
- **POST**: `/api/auth/login`  
   Processes a login request
- **POST**: `/api/auth/register-guest`  
   Processes registering a guest request

### Bookings
- **POST**: `/api/user/bookings`  
   Create a new booking for the current user
- **GET**: `/api/user/bookings`  
   Retrieve a page of bookings for the current user
- **DELETE**: `/api/user/bookings/{id}`  
   Delete an existing booking specified by ID
- **GET**: `/api/user/bookings/{id}`  
   Get a booking specified by ID for the current user
- **GET**: `/api/user/bookings/{id}/invoice`  
   Get the invoice of a booking specified by ID as PDF for the current user

### Cities
- **GET**: `/api/cities`  
   Retrieve a page of cities
- **POST**: `/api/cities`  
   Create a new city
- **GET**: `/api/cities/trending`  
   Returns the top N most visited cities (trending cities)
- **PUT**: `/api/cities/{id}`  
   Update an existing city specified by ID
- **DELETE**: `/api/cities/{id}`  
   Delete an existing city specified by ID
- **PUT**: `/api/cities/{id}/thumbnail`  
   Set the thumbnail of a city specified by ID

### Discounts
- **GET**: `/api/room-classes/{roomClassId}/discounts`  
   Retrieve a page of discounts for a room class
- **POST**: `/api/room-classes/{roomClassId}/discounts`  
   Create a discount for a room class specified by ID
- **GET**: `/api/room-classes/{roomClassId}/discounts/{id}`  
   Get an existing discount by ID
- **PUT**: `/api/room-classes/{roomClassId}/discounts/{id}`  
   Update an existing discount specified by ID

### Hotels
- **GET**: `/api/hotels`  
   Retrieve a page of hotels
- **POST**: `/api/hotels`  
   Create a new hotel
- **GET**: `/api/hotels/search`  
   Search and filter hotels based on specific criteria
- **GET**: `/api/hotels/featured-deals`  
   Retrieve N hotel featured deals
- **GET**: `/api/hotels/{id}`  
   Get hotel by ID for guests
- **PUT**: `/api/hotels/{id}`  
   Update an existing hotel specified by ID
- **DELETE**: `/api/hotels/{id}`  
   Delete an existing hotel specified by ID
- **GET**: `/api/hotels/{id}/room-classes`  
   Get room classes for a hotel specified by ID for Guests
- **PUT**: `/api/hotels/{id}/thumbnail`  
   Set the thumbnail of a hotel specified by ID
- **POST**: `/api/hotels/{id}/gallery`  
   Add a new image to a hotel's gallery specified by ID

### Owners
- **GET**: `/api/owners`  
   Retrieve a page of owners
- **POST**: `/api/owners`  
   Create a new owner
- **GET**: `/api/owners/{id}`  
   Get an existing owner by ID
- **PUT**: `/api/owners/{id}`  
   Update an existing owner

### Reviews
- **GET**: `/api/hotels/{hotelId}/reviews`  
   Retrieve a page of reviews for a hotel specified by ID
- **POST**: `/api/hotels/{hotelId}/reviews`  
   Create a new review for a hotel specified by ID
- **GET**: `/api/hotels/{hotelId}/reviews/{id}`  
   Get a review specified by ID for a hotel specified by ID
- **PUT**: `/api/hotels/{hotelId}/reviews/{id}`  
   Update an existing review specified by ID for a hotel specified by ID
- **DELETE**: `/api/hotels/{hotelId}/reviews/{id}`  
   Delete an existing review specified by ID for a hotel specified by ID

### RoomClasses
- **GET**: `/api/room-classes`  
   Retrieve a page of room classes
- **POST**: `/api/room-classes`  
   Create a new room class
- **PUT**: `/api/room-classes/{id}`  
   Update an existing room class specified by ID
- **DELETE**: `/api/room-classes/{id}`  
   Delete an existing room class specified by ID
- **POST**: `/api/room-classes/{id}/gallery`  
   Add a new image to a room class's gallery specified by ID

### Rooms
- **GET**: `/api/room-classes/{roomClassId}/rooms`  
   Retrieve a page of rooms for a room class
- **POST**: `/api/room-classes/{roomClassId}/rooms`  
   Create a new room in a room class specified by ID
- **GET**: `/api/room-classes/{roomClassId}/rooms/available`  
   Retrieve a page of available rooms for a room class
- **PUT**: `/api/room-classes/{roomClassId}/rooms/{id}`  
   Update an existing room with ID in a room class specified by ID
- **DELETE**: `/api/room-classes/{roomClassId}/rooms/{id}`  
   Delete a room by ID in a room class specified by ID


## Features

### User Authentication
- **Account Creation**: New users can register by submitting their personal details.
- **Secure Login**: Authenticated users can log in securely to access their booking details.

### Image Management System
- **Image Upload and Modification**: Users can upload, update, or remove images related to hotels, cities, and room categories.

### Email Alerts
- **Booking Confirmation**: Automatically sends users confirmation emails once a booking is made, including details like price and hotel location.
- **Timely Updates**: Keeps users informed with reminders and updates on their bookings.

### Admin Control Panel
- **Manage Entities Efficiently**: Admins can search, add, update, and delete entities like cities, hotels, and rooms.
- **Simplified Management**: Provides an intuitive interface for system admins to easily manage tasks.

## Architecture
### Clean Architecture
- **External Layers**: 
    - Web: Handles HTTP requests and responses, directing client interactions to the appropriate services via controllers.
    - Infrastructure: Interfaces with external systems like databases and external services, handling configurations, security, and file operations.
- **Core Layers**:
    - Application Layer: Manages the business logic and coordinates communication across system services.
    - Domain Layer: Contains core business entities and rules, decoupled from external systems for optimal maintainability.

## Technology Stack Overview

### Technologies Used
- **C#**: The backend programming language.
- **ASP.NET Core**: A powerful framework for building robust and scalable web APIs.
- **Entity Framework Core**: Provides ORM capabilities to simplify interactions with the SQL database.
- **SQL Server**: A reliable database management system.
- **Supabase**: Secure cloud-based image storage.
- **Swagger/OpenAPI**: Standardized documentation for API endpoints.
- **JWT**: Handles user authentication and authorization securely.
- **Serilog**: A logging library to track events and monitor performance.

### Security
- **Data Encryption**: Sensitive data is encrypted using `Microsoft.AspNet.Identity.IPasswordHasher` to ensure the security of user credentials.
- 
### Prerequisites
- Install [.NET 8 SDK](https://dotnet.microsoft.com/download).
- A running SQL Server instance.
### Step-by-Step Guide

#### 1. Clone the Repository

Clone the repository of your existing ASP.NET API project to your local machine:

bash
git clone https://github.com/RasheedHendawi/Travel-and-Accommodation-Booking-Platform.git

#### 2. Configure appsettings.json

Open the appsettings.json file located in your project directory and configure the connection string for SQL Server. Replace the <connection_string> placeholder with your SQL Server connection string:

#### 3. Run the API Locally

Start the ASP.NET API project using the following command:

bash
dotnet run


The swagger UI will open automatically where you can try and explore the endpoints or you can open it using http://localhost:<portNumber>/swagger.

##### To access admin's accessability:
- **Email:** Admin@hotelmanager.com
- **Password:** @Admin123


### Contact and Support:
Email: [rasheed.hendawi@gmail.com](mailto:rasheed.hendawi@gmail.com).

[![Build and Test](https://github.com/RasheedHendawi/Travel-and-Accommodation-Booking-Platform/actions/workflows/main.yml/badge.svg)](https://github.com/RasheedHendawi/Travel-and-Accommodation-Booking-Platform/actions/workflows/main.yml)
