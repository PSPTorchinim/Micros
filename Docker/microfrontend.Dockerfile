# Stage 1: Build the application
FROM node:alpine AS builder

# Set working directory
WORKDIR /app

# Add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH

ARG MICROFRONTEND_NAME
ARG FRONTEND_NGINX_FILE

COPY ["Frontends/${MICROFRONTEND_NAME}/package*.json", "./"]
RUN npm config set strict-ssl false
RUN npm install

# Install app dependencies
COPY ["Frontends/${MICROFRONTEND_NAME}/", "./"]
COPY ["${FRONTEND_NGINX_FILE}", "nginx.conf"]

# Start server
CMD ["npm", "run", "start"]