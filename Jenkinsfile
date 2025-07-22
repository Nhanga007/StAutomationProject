pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:8.0' 
            args '-v /var/run/docker.sock:/var/run/docker.sock' 
        }
    }
    stages {
        stage('Checkout') {
            steps {
                git url: 'https://github.com/Nhanga007/StAutomationProject', branch: 'main'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release'
            }
        }
        stage('Test') {
            steps {
                script {
                    docker.image('selenium/standalone-chrome').withRun('-p 4444:4444') { c ->
                        sh 'dotnet test --logger "junit;LogFilePath=test-results.xml"'
                    }
                }
            }
            post {
                always {
                    junit '**/test-results.xml'
                }
            }
        }
    }
}