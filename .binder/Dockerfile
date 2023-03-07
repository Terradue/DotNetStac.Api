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

# Install jupyter extensions
RUN pip install -U "jupyter-server<2.0.0" "jupyter-server-proxy==1.2.0"

#Copy Catalog to data folder
COPY src/Stac.Api.Tests/Resources/TestCatalogs/CatalogS2L2A /data
# Copy supervisor config
COPY ./.binder/supervisord.conf /etc/supervisor/conf.d/supervisord.conf
COPY ./.binder/supervisordserverextension.py /opt/conda/lib/python3.10/site-packages/

# enable supervisord extension
RUN jupyter serverextension enable --sys-prefix supervisordserverextension

# Use local user
RUN chown -R ${NB_UID} ${HOME}
RUN chown -R ${NB_UID} /data
USER ${USER}

# Build the solution
RUN dotnet publish ${HOME}/src/

# Set root to notebooks
WORKDIR ${HOME}/notebooks/