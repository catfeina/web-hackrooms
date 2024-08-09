FROM node:20.16.0-slim
RUN npm install -g @angular/cli@18.0.3
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build
EXPOSE 4200
CMD ["npm", "start"]
