FROM kong
USER 0
RUN cp /etc/kong/kong.conf.default /etc/kong/kong.conf    
ENV KONG_DATABASE="off"
ENV KONG_DECLARATIVE_CONFIG="kong.yml"
RUN echo 'we are running some # of cool things'
COPY . .
USER kong
EXPOSE 8000