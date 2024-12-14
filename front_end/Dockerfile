# Bước 1: Sử dụng Node.js image để build ứng dụng
FROM node:18 AS builder

# Thiết lập thư mục làm việc trong container
WORKDIR /app

# Copy file package.json và package-lock.json để cài đặt dependencies
COPY package*.json ./

# Cài đặt các gói npm
RUN npm install

# Copy toàn bộ mã nguồn vào container
COPY . ./

# Build ứng dụng Vue với Vite
RUN npm run build

# Bước 2: Sử dụng Nginx để phục vụ ứng dụng Vue
FROM nginx:stable-alpine

# Copy các file build từ bước trước vào Nginx
COPY --from=builder /app/dist /usr/share/nginx/html

# Mở cổng 80 cho Nginx
EXPOSE 80

# Khởi chạy Nginx
CMD ["nginx", "-g", "daemon off;"]
