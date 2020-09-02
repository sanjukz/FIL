## Deployment

We use Jenkins for CI.

* Default Region is currently US West (Oregon)
* On Jenkins Machine
    * Add Environment Variable: AWS_ACCESS_KEY_ID as the deployment users Access Key
    * Add Environment Variable: AWS_SECRET_ACCESS_KEY as the Secret Key
* For master and develop, we will call Build/deploy/RunDeploy.ps1 with the branch name to do builds

* We are using AWS System Manager Parameter Store for secret variable storage
    * IAM KMS key created named Deployment
    * These exist namespaces by naming convention to environments, so develop and master
    * Go to the System Manager console to see whats installed (or read the RunDeploy.ps1 code)

* Steps:
    * RunBuild.ps1
    * RunTest.ps1
        * Runs all test projects  (*.Test.*)
    * If develop or master
        * RunMigration.ps1 <branch name>
            * Runs both migrators
        * RunDeploy.ps1 <branch name> <region: optional - us-west-2>
            * Deletes all site-deploy folders
            * publishes to site-deploy
            * Generates LB file for Web projects
            * Generates environment variables for all

* Users
    * Deployment
        * SES, S3 and KMS access for Development Key
    * Deploy
        * EB, RDS, KMS Deployment key access
    * Production
        * SES, S3 and KMS Key for Production Key

## TODO
    * SSL configuration for web projects