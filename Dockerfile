FROM kong
USER 0
COPY kong.yml /
RUN cp /etc/kong/kong.conf.default /etc/kong/kong.conf    
COPY . .
USER kong
EXPOSE 8000