FROM gitpod/workspace-full:latest

# Instalacja .NET SDK 9.0
RUN wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh \
    && chmod +x dotnet-install.sh \
    && ./dotnet-install.sh --version latest --install-dir /usr/share/dotnet \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet-install.sh

# Instalacja Node.js i npm (dla React)
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs

# Instalacja globalnych narzędzi npm
RUN npm install -g npm@latest

# Dodanie zmiennych środowiskowych
ENV DOTNET_ROOT=/usr/share/dotnet \
    PATH="$PATH:/usr/share/dotnet"

# Weryfikacja instalacji
RUN dotnet --version && \
    node --version && \
    npm --version
