pipeline {
    agent any

    environment {
        SONAR_TOKEN = credentials('sonar-token-jenkins')
        DOTNET_ROOT = "${WORKSPACE}/.dotnet"
        PATH = "${WORKSPACE}/.dotnet:${WORKSPACE}/.dotnet/tools:${env.PATH}"
    }

    stages {

        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Install system dependencies') {
            steps {
                sh '''
                    apt-get update
                    apt-get install -y libicu-dev libssl-dev libcurl4-openssl-dev zlib1g libkrb5-3
                '''
            }
        }

        stage('Setup .NET 9.0') {
            steps {
                sh '''
                    mkdir -p $DOTNET_ROOT
                    curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
                    chmod +x dotnet-install.sh
                    ./dotnet-install.sh --channel 9.0 --install-dir $DOTNET_ROOT
                    $DOTNET_ROOT/dotnet --version
                '''
            }
        }

        stage('Install SonarScanner') {
            steps {
                sh '''
                    dotnet tool install --global dotnet-sonarscanner || dotnet tool update --global dotnet-sonarscanner
                '''
            }
        }

        stage('Analyze') {
            steps {
                script {
                    def prArgs = env.CHANGE_ID ?
                    "/d:sonar.pullrequest.key=${env.CHANGE_ID} /d:sonar.pullrequest.branch=${env.CHANGE_BRANCH} /d:sonar.pullrequest.base=${env.CHANGE_TARGET}" :
                    "/d:sonar.branch.name=${env.BRANCH_NAME ?: 'main'}"

                    sh """
                        dotnet sonarscanner begin \
                          /k:"AndrodenBY_Imaginator" \
                          /o:"androdenby" \
                          /d:sonar.login="${SONAR_TOKEN}" \
                          /d:sonar.host.url="https://sonarcloud.io" \
                          ${prArgs}

                        dotnet restore Imaginator.sln
                        dotnet build Imaginator.sln --no-restore --configuration Release

                        dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"
                    """
                }
            }
        }
    }
}
