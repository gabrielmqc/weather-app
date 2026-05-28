FROM node:22-alpine AS deps
WORKDIR /app

RUN corepack enable

COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile


FROM node:20-alpine AS builder
WORKDIR /app

RUN corepack enable

ARG VITE_API_URL_PROD
ENV VITE_API_URL_PROD=$VITE_API_URL_PROD

COPY --from=deps /app/node_modules ./node_modules
COPY . .

RUN yarn build


FROM nginx:alpine

COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
