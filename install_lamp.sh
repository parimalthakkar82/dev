#!/bin/bash
###### New check in newbranch1
# Function to install a package if it's not already installed
install_package() {
    PACKAGE_NAME=$1
    if ! dpkg -l | grep -q "^ii  $PACKAGE_NAME "; then
        echo "$PACKAGE_NAME is not installed. Installing..."
        sudo apt-get install -y $PACKAGE_NAME
    else
        echo "$PACKAGE_NAME is already installed."
    fi
}

# Update package index
echo "Updating package index..."
sudo apt-get update

# Check and install Apache
install_package "apache2"

# Check and install MySQL
install_package "mysql-server"

# Secure MySQL installation
if [ ! -f /var/lib/mysql/.mysql_secure_installation ]; then
    echo "Securing MySQL installation..."
    sudo mysql_secure_installation
    sudo touch /var/lib/mysql/.mysql_secure_installation
fi

# Check and install PHP
install_package "php"
install_package "libapache2-mod-php"
install_package "php-mysql"

# Create PHP info page to verify PHP installation
INFO_PAGE="/var/www/html/info.php"
if [ ! -f $INFO_PAGE ]; then
    echo "Creating PHP info page..."
    echo "<?php phpinfo(); ?>" | sudo tee $INFO_PAGE
else
    echo "PHP info page already exists."
fi

# Restart Apache to apply changes
echo "Restarting Apache..."
sudo systemctl restart apache2

# Check services status
echo "Checking services status..."
sudo systemctl status apache2
sudo systemctl status mysql

# Output completion message
echo "LAMP stack installation and configuration complete."
echo "Verify PHP by visiting http://your_server_ip/info.php"

# Open firewall for Apache if UFW is enabled
if sudo ufw status | grep -q "Status: active"; then
    echo "Allowing Apache through UFW..."
    sudo ufw allow in "Apache Full"
    sudo ufw reload
fi

# Check if Apache, MySQL, and PHP are running
echo "Verifying that Apache, MySQL, and PHP are operational..."
if systemctl is-active --quiet apache2 && systemctl is-active --quiet mysql && php -v > /dev/null 2>&1; then
    echo "All LAMP stack components are installed and running."
else
    echo "One or more LAMP stack components are not running. Please check the services."
fi
