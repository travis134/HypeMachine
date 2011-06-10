<?php

class Dom_Singleton extends DOMDocument
{
    public function &instance($version = "1.0")
    {
    	static $instance = null;
        if($instance === null)
        {
            try
            {
                $instance = new self($version);
            }
            catch(DOMException $e)
            {
                //Properly handle exception
            }
        }
        return $instance;
    }
}

?>