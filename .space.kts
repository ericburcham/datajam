/**
* JetBrains Space Automation
* This Kotlin-script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/

val dotNetInstallScript = """
    apt-get update && apt-get install -y apt-utils apt-transport-https
    apt-get install -y curl unzip wget software-properties-common git
    
    wget https://packages.microsoft.com/config/ubuntu/22.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    apt-get update
    apt-get install -y dotnet-sdk-3.1 dotnet-sdk-6.0
""".trimIndent()

job("Continuous integration build") {
    startOn {
        gitPush { enabled = true }
    }
    
    container("mcr.microsoft.com/dotnet/sdk:6.0-focal") {
        resources {
            cpu = 2.cpu
            memory = 2.gb
        }
        
        shellScript {
            content = dotNetInstallScript + """            
            	./build.sh
            """.trimIndent()
        }
    }
}