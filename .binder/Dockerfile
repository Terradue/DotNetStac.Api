FROM terradue/dotnetinteractive:latest

USER root

# Copy package sources
COPY ./.binder/NuGet.config ${HOME}/nuget.config

# Install software to run catalog in background
RUN apt-get update \
    && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends \
        supervisor \
    && rm -rf /var/lib/apt/lists/*

# Copy project source
COPY ./src/ ${HOME}/src/
# Copy notebooks
COPY ./notebooks/ ${HOME}/notebooks/

RUN chown -R ${NB_UID} ${HOME}
USER ${USER}

RUN dotnet publish ${HOME}/src/

COPY src/Stac.Api.Tests/Resources/TestCatalogs/CatalogS2L2A /data
# Copy supervisor config
COPY ./.binder/supervisord.conf /etc/supervisor/conf.d/supervisord.conf

# Set root to notebooks
WORKDIR ${HOME}/notebooks/