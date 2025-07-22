pipeline {
    agent any // Chạy trên bất kỳ agent nào, không phụ thuộc container ngay từ đầu

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
                    // Chạy Selenium Grid trong container
                    bat 'docker run -d -p 4444:4444 --name selenium-chrome selenium/standalone-chrome'
                    // Chạy kiểm thử trong môi trường hiện tại với RemoteWebDriver
                    bat 'dotnet test --logger "junit;LogFilePath=test-results.xml" --filter TestCategory!=Ignore'
                }
            }
            post {
                always {
                    junit '**/test-results.xml' // Hiển thị kết quả kiểm thử
                    // Dừng và xóa container Selenium sau khi test
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