FROM kong
USER 0
RUN cp /etc/kong/kong.conf.default /etc/kong/kong.conf    
COPY . .
ENV KONG_DATABASE="off"
ENV KONG_DECLARATIVE_CONFIG="kong.yml"
USER kong
EXPOSE 8000