# 🐶 Dog API Microservice
The DogApiMicroservice is a robust and scalable microservice designed to interact with the Dog API via secured endpoints. This microservice wraps the existing Dog API behind a custom API, providing enhanced functionality and control. It is containerized using Docker and adheres to industry best practices, including unit testing and build pipelines.

## 🚀 Features Implemented

✅ **API Endpoints**
- Fetch all dog breeds (`ListAllBreeds`)
- Fetch a random dog image (`RandomBreedImage`)
- Fetch random dog images by breed (`RandomBreedImageByBreed`)
- User authentication and JWT token generation (`Auth.GenerateTokenAsync`)

✅ **API Docs**
- Generate documentation for endpoints.

✅ **Fluent Validation**
- Validate incoming requests and generate appropriate resposne codes

✅ **Security**
- Authentication & Authorization using **JWT**
- Secure endpoints with **role-based access control (RBAC)**
- Password hashing using **SHA256 with salt**

✅ **Feature Flags**
- Uses **Flagsmith** to control feature rollouts dynamically

✅ **Logging**
- Uses **Serilog for structured logging**
- Logs are stored in **Seq for log management**
- Supports both **local development and Docker-based Seq logging**

✅ **Docker**
- Builds into an **Alpine-based container** for efficiency
- Uses **Docker Compose** to manage **Dog API + Seq**
- Runs as a **non-root user** for security
- **.dockerignore** used to keep the image size small

✅ **Code Quality**
- Uses **StyleCop** for code style enforcement
- Uses **SonarAnalyzer.CSharp** for static code analysis

 

## 🚀 Running the Project

### 🖥️ Running with Visual Studio (Recommended)
1. **Ensure Docker is running**  
   - Open **Docker Desktop** and make sure it's running in the background.

2. **Open the solution in Visual Studio**  
   - Navigate to the `DogApi.sln` file and open it in **Visual Studio**.

3. **Select "Docker Compose" as the startup project**  
   - Click on the **Startup Project dropdown** (next to the "Run" button in VS).
   - Select **"Docker Compose"** .  

4. **Run the application**  
   - Click the **"Docker Compose"** button or press `F5` to start the containers.
   - Visual Studio will:
     - Build the **Docker image**
     - Start the **Dog API service**
     - Start the **Seq service for logging**

5. **Verify the running services**
   - Open your browser and check:
     - **API Swagger UI**: [http://localhost:5000/swagger](http://localhost:5000/swagger)
     - **Seq Logging UI**: [http://localhost:5341](http://localhost:5341)



## Pending Work

### Testing
- Unit tests (authentication, endpoints) - In progress
- Integration tests - Not started
- End-to-end tests - Not started

### CI/CD Pipeline
- Add Azure Pipelines for automated builds
- Run unit tests before merging PRs
- Implement code coverage reports
- Automate Docker image publishing

