<?php

require_once 'Dom_Singleton.class.php';

class Xml_Serializable_Object
{	
	public function serialize()
	{
		$dom = Dom_Singleton::instance();
		
		$item = $dom->createElement(get_class($this));
		
		$properties = get_object_vars($this);
		foreach($properties as $name => $value)
		{
			$item->appendChild($dom->createElement($name, $value));
		}
		
		return $item;
	}
}

?>