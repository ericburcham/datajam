// Sets the container image to use when running the build.
val buildContainerImage = "ubuntu:22.04"

// Sets the build script to execute for the continuous integration build.
val buildScript = """
    # Update apt and install necessary Linux tools.
    apt-get update && apt-get install -y apt-utils apt-transport-https
    apt-get install -y curl unzip wget software-properties-common git

    # Install the dotnet frameworks and command lines we need.
    wget https://dot.net/v1/dotnet-install.sh
    chmod +x ./dotnet-install.sh
    ./dotnet-install.sh --channel 6.0
    PATH=${'$'}PATH:${'$'}HOME/.dotnet:${'$'}HOME/.dotnet/tools
    dotnet --list-sdks

    # Register the space nuget feed.
    dotnet nuget add source {{ project:msa_nuget_space_target_url }} -n space -u "%JB_SPACE_CLIENT_ID%" -p "%JB_SPACE_CLIENT_SECRET%" --store-password-in-clear-text

    # Execute the Nuke build.
    ./build.sh
""".trimIndent()

// Sets the build script to execute for the RELEASE build.
val releaseBuildScript = """
    # Update apt and install necessary Linux tools.
    apt-get update && apt-get install -y apt-utils apt-transport-https
    apt-get install -y curl unzip wget software-properties-common git

    # Install the dotnet frameworks and command lines we need.
    wget https://dot.net/v1/dotnet-install.sh
    chmod +x ./dotnet-install.sh
    ./dotnet-install.sh --channel 6.0
    PATH=${'$'}PATH:${'$'}HOME/.dotnet:${'$'}HOME/.dotnet/tools
    dotnet --list-sdks

    # Execute the Nuke build.
    ./build.sh
""".trimIndent()

job("Continuous Integration Build") {
    startOn {
        gitPush {
            enabled = true

            // Enable for all git flow branches.
            branchFilter {
                // Develop branch
                +"develop"

                // Git-flow branch prefixes
                +"bugfix/*"
                +"feature/*"
                +"hotfix/*"
                +"release/*"
                +"support/*"
            }
        }
    }

    container(buildContainerImage) {
        resources {
            cpu = 2.cpu
            memory = 4.gb
        }

        // Disable unneeded dotnet stuff.
        env.set("DOTNET_NOLOGO", "true")
        env.set("DOTNET_SKIP_FIRST_TIME_EXPERIENCE", "true")
        env.set("DOTNET_CLI_TELEMETRY_OPTOUT", "true")

        // Set environment variables.
        env.set("NuGetSpaceTargetApiKey", "{{ project:msa_nuget_space_api_key }}")
        env.set("NuGetSpaceTargetUrl", "{{ project:msa_nuget_space_target_url }}")

        shellScript {
            content = buildScript
        }
    }
}

job("Release Build") {
    startOn {
        gitPush {
            enabled = true

            // Enable for all git flow branches.
            branchFilter {
                // Main branch
                +"main"
            }
        }
    }

    container(buildContainerImage) {
        resources {
            cpu = 2.cpu
            memory = 4.gb
        }

        // Disable unneeded dotnet stuff.
        env.set("DOTNET_NOLOGO", "true")
        env.set("DOTNET_SKIP_FIRST_TIME_EXPERIENCE", "true")
        env.set("DOTNET_CLI_TELEMETRY_OPTOUT", "true")

        // Set environment variables.
        env.set("NuGetSpaceTargetApiKey", "{{ project:msa_nuget_space_api_key }}")
        env.set("NuGetSpaceTargetUrl", "{{ project:msa_nuget_space_target_url }}")

        env.set("NuGetOrgTargetApiKey", "{{ project:msa_nuget_api_key }}")
        env.set("NuGetOrgTargetUrl", "{{ project:msa_nuget_target_url }}")

        shellScript {
            content = releaseBuildScript
        }
    }
}
