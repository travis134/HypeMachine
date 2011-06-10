<?php

require_once 'config.php';

class Db_Singleton extends PDO
{
    public function &instance($dsn = null, $username = null, $password = null, $driver_options = null)
    {
		global $DB_TYPE, $HOST_NAME, $DB_NAME, $USER_NAME, $PASSWORD, $DRIVER_OPTIONS;
		
		if(is_null($dsn))
		{
			$dsn = "$DB_TYPE:host=$HOST_NAME;dbname=$DB_NAME";
		}
		if(is_null($username))
		{
			$username = $USER_NAME;
		}
		if(is_null($password))
		{
			$password = $PASSWORD;
		}
		if(is_null($driver_options))
		{
			$driver_options = $DRIVER_OPTIONS;
		}
		
        static $instance = null;
        if($instance === null)
        {
            try
            {
                $instance = new self($dsn, $username, $password, $driver_options);
            }
            catch(PDOException $e)
            {
                //Properly handle exception
            }
        }
        return $instance;
    }
}

?>