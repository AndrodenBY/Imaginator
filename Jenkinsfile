pipeline {
    agent any

    environment {
        SONAR_TOKEN = credentials('sonar-token-jenkins')
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
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
                        echo "PR build: #${env.CHANGE_ID}"
                        prArgs = "/d:sonar.pullrequest.key=${env.CHANGE_ID} /d:sonar.pullrequest.branch=${env.CHANGE_BRANCH} /d:sonar.pullrequest.base=${env.CHANGE_TARGET}"
                    } else if (env.ghprbPullId) {
                        echo "PR build (GHPRB): #${env.ghprbPullId}"
                        prArgs = "/d:sonar.pullrequest.key=${env.ghprbPullId} /d:sonar.pullrequest.branch=${env.ghprbSourceBranch} /d:sonar.pullrequest.base=${env.ghprbTargetBranch}"
                    } else {
                        def b = env.BRANCH_NAME ?: 'main'
                        echo "Branch build: ${b}"
                        prArgs = "/d:sonar.branch.name=${b}"
                    }

                    sh """
                        export PATH="\\\$PATH:/root/.dotnet/tools"

                        dotnet sonarscanner begin \
                          /k:"AndrodenBY_Imaginator" \
                          /o:"androdenby" \
                          /d:sonar.login="\\\${SONAR_TOKEN}" \
                          /d:sonar.host.url="https://sonarcloud.io" \
                          /d:sonar.exclusions="**/Migrations/**,**/Dockerfile,**/*appsettings*.json" \
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
