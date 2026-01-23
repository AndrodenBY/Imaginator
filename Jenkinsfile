pipeline {
    agent any

    environment {
        SONAR_TOKEN = credentials('sonar-token-jenkins')

        // Local .NET installation directory
        DOTNET_ROOT = "${WORKSPACE}/.dotnet"

        // Add local dotnet + local tools + global tools + system PATH
        PATH = "${WORKSPACE}/.dotnet:${WORKSPACE}/.dotnet/tools:/root/.dotnet/tools:${env.PATH}"
    }

    stages {

        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Install system dependencies (ARM64)') {
            steps {
                sh '''
                    set -e
                    apt-get update
                    apt-get install -y \
                        libicu-dev \
                        libssl-dev \
                        libcurl4-openssl-dev \
                        zlib1g \
                        libkrb5-3
                '''
            }
        }

        stage('Setup .NET 9 SDK') {
            steps {
                sh '''
                    set -e
                    mkdir -p "$DOTNET_ROOT"
                    curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
                    chmod +x dotnet-install.sh

                    ./dotnet-install.sh --channel 9.0 --install-dir "$DOTNET_ROOT"

                    echo "Installed .NET version:"
                    "$DOTNET_ROOT/dotnet" --version
                '''
            }
        }

        stage('Install SonarScanner') {
            steps {
                sh '''
                    set -e
                    export DOTNET_ROOT="$DOTNET_ROOT"
                    export PATH="$DOTNET_ROOT:$DOTNET_ROOT/tools:/root/.dotnet/tools:$PATH"

                    dotnet tool install --global dotnet-sonarscanner \
                      || dotnet tool update --global dotnet-sonarscanner

                    echo "Tools installed in /root/.dotnet/tools:"
                    ls -la /root/.dotnet/tools
                '''
            }
        }

        stage('Analyze') {
            steps {
                script {
                    def prArgs = ''

                    if (env.CHANGE_ID) {
                        echo "PR build (Branch Source): #${env.CHANGE_ID}"
                        prArgs =
                                "/d:sonar.pullrequest.key=${env.CHANGE_ID} " +
                                "/d:sonar.pullrequest.branch=${env.CHANGE_BRANCH} " +
                                "/d:sonar.pullrequest.base=${env.CHANGE_TARGET}"
                    } else if (env.ghprbPullId) {
                        echo "PR build (GHPRB): #${env.ghprbPullId}"
                        prArgs =
                                "/d:sonar.pullrequest.key=${env.ghprbPullId} " +
                                "/d:sonar.pullrequest.branch=${env.ghprbSourceBranch} " +
                                "/d:sonar.pullrequest.base=${env.ghprbTargetBranch}"
                    } else {
                        def branch = env.BRANCH_NAME ?: 'main'
                        echo "Branch build: ${branch}"
                        prArgs = "/d:sonar.branch.name=${branch}"
                    }

                    sh """
                        set -e
                        export DOTNET_ROOT="\$DOTNET_ROOT"
                        export PATH="\$DOTNET_ROOT:\$DOTNET_ROOT/tools:/root/.dotnet/tools:\$PATH"

                        dotnet sonarscanner begin \\
                          /k:"AndrodenBY_Imaginator" \\
                          /o:"androdenby" \\
                          /d:sonar.login="\\\${SONAR_TOKEN}" \\
                          /d:sonar.host.url="https://sonarcloud.io" \\
                          /d:sonar.exclusions="**/Migrations/**,**/Dockerfile,**/*appsettings*.json" \\
                          ${prArgs}

                        dotnet restore Imaginator.sln
                        dotnet build Imaginator.sln --no-restore --configuration Release

                        dotnet sonarscanner end /d:sonar.login="\\\${SONAR_TOKEN}"
                    """
                }
            }
        }
    }
}