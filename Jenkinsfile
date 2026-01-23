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

        stage('Setup .NET 9.0 (like GitHub Actions)') {
            steps {
                sh '''
                    echo "Installing .NET 9.0 SDK..."

                    mkdir -p $DOTNET_ROOT
                    curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
                    chmod +x dotnet-install.sh

                    ./dotnet-install.sh --channel 9.0 --install-dir $DOTNET_ROOT

                    echo "Installed dotnet version:"
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
                    def prArgs = ''

                    if (env.CHANGE_ID) {
                        prArgs = "/d:sonar.pullrequest.key=${env.CHANGE_ID} /d:sonar.pullrequest.branch=${env.CHANGE_BRANCH} /d:sonar.pullrequest.base=${env.CHANGE_TARGET}"
                    } else {
                        prArgs = "/d:sonar.branch.name=${env.BRANCH_NAME ?: 'main'}"
                    }

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
