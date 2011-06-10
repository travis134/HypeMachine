<?php

header("Content-type: text/xml");

require_once 'Hype.class.php';
require_once 'Aftermath.class.php';
require_once 'Comment.class.php';
require_once 'User.class.php';
require_once 'Game.class.php';

require_once 'Dom_Singleton.class.php';

$dom = Dom_Singleton::instance();
$root = $dom->createElement("Items");
$dom->appendChild($root);

$intent = $_GET['intent'];
$args = $_GET['args'];

switch($intent)
{
	default:
	case 0:
		$hypes = Hype::readAllWithValue($args);

		for($i = 0; $i<count($hypes); $i++)
		{
			$root->appendChild($hypes[$i]->serialize());
		}
		break;
	case 1:
		$aftermaths = Aftermath::readAllWithValue($args);

		for($i = 0; $i<count($aftermaths); $i++)
		{
			$root->appendChild($aftermaths[$i]->serialize());
		}
		break;
	case 2:
		$comments = Comment::readAllWithValue($args);

		for($i = 0; $i<count($comments); $i++)
		{
			$root->appendChild($comments[$i]->serialize());
		}
		break;
	case 3:
		$users = User::readAll();

		for($i = 0; $i<count($users); $i++)
		{
			$root->appendChild($users[$i]->serialize());
		}
		break;
	case 4:
		$games = Game::readAll();

		for($i = 0; $i<count($games); $i++)
		{
			$root->appendChild($games[$i]->serialize());
		}
		break;
}

echo $dom->saveXML();

?>