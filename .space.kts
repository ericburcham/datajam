/**
* JetBrains Space Automation
* This Kotlin-script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/

val buildContainerImage = "ubuntu:22.04"
val buildScript = """
    apt-get update && apt-get install -y apt-utils apt-transport-https
    apt-get install -y curl unzip wget software-properties-common git
    wget https://dot.net/v1/dotnet-install.sh
    chmod +x ./dotnet-install.sh
    ./dotnet-install.sh --channel 6.0
    ./dotnet-install.sh --channel 7.0
    PATH=${'$'}PATH:${'$'}HOME/.dotnet:${'$'}HOME/.dotnet/tools
    dotnet --list-sdks
    ./build.sh
""".trimIndent()

job("Continuous integration build") {
    startOn {
        gitPush {
            enabled = true

            branchFilter {
                -"refs\heads\feature\*"
            }
        }
    }
    
    container(buildContainerImage) {
        resources {
            cpu = 2.cpu
            memory = 4.gb
        }
        
        shellScript {
            content = buildScript
        }
    }
}