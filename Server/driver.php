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
//$args = $_GET['args'];
$guids = $_GET['guids'];

$user_args = $_GET['user_args'];
$game_args = $_GET['game_args'];
$aftermath_args = $_GET['aftermath_args'];
$hype_args = $_GET['hype_args'];
$comment_args = $_GET['comment_args'];

switch($intent)
{
	default:
		break;
	case 'createUser':
		$user = new User(array('anid' => $user_args['anid'],  'nickname' => $user_args['nickname']));
		if($user->getExists())
		{
			$root->appendChild($user->serialize());
		}
		break;
	case 'readUser':
		$user = new User(array('anid' => $user_args['anid']));
		if($user->getExists())
		{
			$root->appendChild($user->serialize());
		}
		break;
	case 'updateUser':
		$user = new User(array('id' => $user_args['id']));
		if($user->getExists())
		{
			if(isset($user_args['nickname']))
			{
				$user->setNickname($user_args['nickname']);
			}
			if(isset($user_args['anid']))
			{
				$user->setAnid($user_args['anid']);
			}
			$user->update();
			$user->read();
			$root->appendChild($user->serialize());
		}
		break;
	case 'deleteUser':
		$user = new User(array('id' => $user_args['id']));
		if($user->getExists())
		{
			$user->delete();
		}
		break;
	case 'createGame':
	case 'readGame':
		$game = new Game(array('guid' => $game_args['guid']));
		if($game->getExists())
		{
			$root->appendChild($game->serialize());
		}
		break;
	case 'updateGame':
		$game = new Game(array('id' => $user_args['id']));
		if($game->getExists())
		{
			if(isset($game_args['guid']))
			{
				$game->setGuid($game_args['guid']);
			}
			$user->update();
			$user->read();
			$root->appendChild($user->serialize());
		}
		break;
	case 'deleteGame':
		$game = new Game(array('id' => $user_args['id']));
		if($game->getExists())
		{
			$game->delete();
		}
		break;
	case 'createAftermath':
		$aftermath = new Aftermath(array('game_id' => $aftermath_args['game_id'],  'user_id' => $aftermath_args['user_id'],  'score' => $aftermath_args['score']));
		if($aftermath->getExists())
		{
			$root->appendChild($aftermath->serialize());
		}
		break;
	case 'readAftermath':
		$aftermath = new Aftermath(array('game_id' => $aftermath_args['game_id'],  'user_id' => $aftermath_args['user_id']));
		if($aftermath->getExists())
		{
			$root->appendChild($aftermath->serialize());
		}
		break;
	case 'updateAftermath':
		$aftermath = new Aftermath(array('id' => $aftermath_args['id']));
		if($aftermath->getExists())
		{
			if(isset($aftermath_args['game_id']))
			{
				$aftermath->setGameId($aftermath_args['game_id']);
			}
			if(isset($aftermath_args['aftermath_id']))
			{
				$aftermath->setUserId($aftermath_args['aftermath_id']);
			}
			if(isset($aftermath_args['score']))
			{
				$aftermath->setScore($aftermath_args['score']);
			}
			$aftermath->update();
			$aftermath->read();
			$root->appendChild($aftermath->serialize());
		}
		break;
	case 'deleteAftermath':
		$aftermath = new Aftermath(array('id' => $aftermath_args['id']));
		if($aftermath->getExists())
		{
			$aftermath->delete();
		}
		break;
	case 'createHype':
		$hype = new Hype(array('game_id' => $hype_args['game_id'],  'user_id' => $hype_args['user_id'],  'score' => $hype_args['score']));
		if($hype->getExists())
		{
			$root->appendChild($hype->serialize());
		}
		break;
	case 'readHype':
		$hype = new Hype(array('game_id' => $hype_args['game_id'],  'user_id' => $hype_args['user_id']));
		if($hype->getExists())
		{
			$root->appendChild($hype->serialize());
		}
		break;
	case 'updateHype':
		$hype = new Hype(array('id' => $hype_args['id']));
		if($hype->getExists())
		{
			if(isset($hype_args['game_id']))
			{
				$hype->setGameId($hype_args['game_id']);
			}
			if(isset($hype_args['user_id']))
			{
				$hype->setUserId($hype_args['user_id']);
			}
			if(isset($hype_args['score']))
			{
				$hype->setScore($hype_args['score']);
			}
			$hype->update();
			$hype->read();
			$root->appendChild($hype->serialize());
		}
		break;
	case 'deleteHype':
		$hype = new Hype(array('id' => $hype_args['id']));
		if($hype->getExists())
		{
			$hype->delete();
		}
		break;
	case 'createComment':
		$comment = new Comment(array('game_id' => $comment_args['game_id'],  'user_id' => $comment_args['user_id'],  'content' => $comment_args['content']));
		if($comment->getExists())
		{
			$root->appendChild($comment->serialize());
		}
		break;
	case 'readComment':
		$comment = new Comment(array('game_id' => $comment_args['game_id'],  'user_id' => $comment_args['user_id']));
		if($comment->getExists())
		{
			$root->appendChild($comment->serialize());
		}
		break;
	case 'updateComment':
		$comment = new Comment(array('id' => $comment_args['id']));
		if($comment->getExists())
		{
			if(isset($comment_args['game_id']))
			{
				$comment->setGameId($comment_args['game_id']);
			}
			if(isset($comment_args['user_id']))
			{
				$comment->setUserId($comment_args['user_id']);
			}
			if(isset($comment_args['content']))
			{
				$comment->setContent($comment_args['content']);
			}
			$comment->update();
			$comment->read();
			$root->appendChild($comment->serialize());
		}
		break;
	case 'deleteComment':
		$comment = new Comment(array('id' => $comment_args['id']));
		if($comment->getExists())
		{
			$comment->delete();
		}
		break;
	case 'readAll':
		$user = new User(array('anid' => $user_args['anid'],  'nickname' => $user_args['nickname']));
		if($user->getExists())
		{
			$root->appendChild($user->serialize());
		}
		$users = array();
		$games = Game::readAllWithGuids($guids);
		for($i = 0; $i<count($games); $i++)
		{
			$root->appendChild($games[$i]->serialize());
			$aftermaths = Aftermath::readAllWithValue(array('game_id' => $games[$i]->getId()));
			$hypes = Hype::readAllWithValue(array('game_id' => $games[$i]->getId()));
			$comments = Comment::readAllWithValue(array('game_id' => $games[$i]->getId()));
			for($j = 0; $j<count($aftermaths); $j++)
			{
				$root->appendChild($aftermaths[$j]->serialize());				
			}
			for($j = 0; $j<count($hypes); $j++)
			{
				$root->appendChild($hypes[$j]->serialize());
			}
			for($j = 0; $j<count($comments); $j++)
			{
				$root->appendChild($comments[$j]->serialize());
				$temp = new User(array('id' => $comments[$j]->getUserId()));
				if($temp->getExists() && !in_array($temp, $users))
				{
					$users[$temp->getId()] = $temp;
				};	
			}
		}
		foreach($users as $user)
		{
			$root->appendChild($user->serialize());
		}
		break;
}

echo $dom->saveXML();

?>