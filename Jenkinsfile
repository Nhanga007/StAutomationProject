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
            // Stop & remove container nếu đang chạy
            sh '''
                if [ $(docker ps -aq -f name=selenium-chrome) ]; then
                    docker rm -f selenium-chrome || true
                fi
            '''

            // Khởi chạy lại container mới
            sh 'docker run -d -p 4444:4444 --name selenium-chrome selenium/standalone-chrome'

            // Chạy test
            sh 'dotnet test --logger "junit;LogFilePath=test-results.xml"'
        }
    }
    post {
        always {
            junit '**/test-results.xml'
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