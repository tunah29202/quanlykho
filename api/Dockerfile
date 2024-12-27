# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Tạo thư mục để chứa ứng dụng
WORKDIR /app

# Copy file csproj và restore các dependencies
COPY *.csproj ./ 
RUN dotnet restore

# Copy toàn bộ mã nguồn vào container
COPY . ./

# Build ứng dụng
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime (Chỉ sử dụng runtime, không cần SDK)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Tạo thư mục runtime để chạy ứng dụng
WORKDIR /app

# Copy file đã build từ giai đoạn "build"
COPY --from=build /app/publish . 

# Expose cổng 80 để lắng nghe HTTP request
EXPOSE 80

# Thiết lập lệnh khởi chạy ứng dụng
ENTRYPOINT ["dotnet", "api.dll"]
