pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                git url: 'https://github.com/Nhanga007/StAutomationProject.git', branch: 'main'
            }
        }
        stage('Build') {
            steps {
                bat 'dotnet restore'
                bat 'dotnet build --configuration Release'
            }
        }
        stage('Test') {
            steps {
                script {
                    bat 'docker run -d -p 4444:4444 --name selenium-chrome selenium/standalone-chrome'
                    bat 'dotnet test --logger "junit;LogFilePath=test-results.xml" --filter TestCategory!=Ignore'
                }
            }
            post {
                always {
                    junit '**/test-results.xml'
                    bat 'docker stop selenium-chrome && docker rm selenium-chrome'
                }
            }
        }
    }
    post {
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed!'
        }
    }
}