<!-- variables -->
<!-- [disclaimer]: src="" can't use markdown variables as in html tags as attributes placeholders or whatever -->

<!-- logo -->
<!-- <p align="center">an innovative augmented reality game that stimulates the mind</p> -->
<p align="center" href="#">
  <img align="center" height="500" src="Resources/ui/Icon/Icon.png" />
</p>

<!-- badges -->
<div align="center">
<a href="https://github.com/ellerbrock/open-source-badges">
<img src="https://badges.frapsoft.com/os/v1/open-source.png?v=103" />
</a>
<a href="https://opensource.org/licenses/mit-license.php">
<img src="https://badges.frapsoft.com/os/mit/mit.svg?v=103" />
</a>
</div>

<!-- disclaimer -->
<br /><img  width="175" src="https://img.shields.io/badge/Disclaimer-blue?style=for-the-badge" />

This project was done in 3 months in 2017 under emmense pressure and stress so take it with a grain of awh.


<br /><img width="175" src="https://img.shields.io/badge/How%20To%20Play-blue?style=for-the-badge">

The game has only been compiled as a `.apk` so you it can be played on android only

<br /><img width="100" src="https://img.shields.io/badge/Specs-red?style=for-the-badge">
- Android version > Kitkat 4.6 (API 19)
- Quad Core chip-set processor
- Rear Camera 10MP
in most cases you'll be able to play the game just fine now adays

<br /><img width="100" src="https://img.shields.io/badge/Steps-red?style=for-the-badge">
- Download or directly print the [playfield](Resources/game%20elements/GPF%20Official.png)
- Download and Install the latest `.apk` from releases

I'd recommend you watch the [demo video]() to get a firm idea of what the game is and how it's played
---

# Content
- [Introduction](#Introduction)
- [Technology Review](#Technology_Review)
- [Game Concept](#Game_Concept)

<!-- 3.0 Game Designs Imparted with AR
3.1 My Game Concept
3.2 Game Modes
3.3 Game Rules & Elements
3.3 Platform -->


# Introduction
Making a game was never an easy task because the ultimate goal is to amuse people, but, building a video game with all graphics and controllers has raised the bar a little higher. The creation of a game that interacts with player’s environment takes our imagination a little further towards the future of entertaining. This project explores a unique game concept where the phone is nothing but a middleman and whoever is playing has to move physically within the room to move objects in the game.

# Technology Review
Back in 2017, it was a little hard to find an open source AR library that fits my requirements and the devices i had at hand to construct and deliver this game and its concept. luckily there was 3 libraries and tools that gave this project a glimpse of hope and helped with my graduation project and those are:
- [Vuforia](https://developer.vuforia.com)
- [ARToolKit](http://www.artoolkitx.org)
- [IN2AR](https://www.as3gamegears.com/augmented-reality/in2ar/) \
Eventually i chose Vuforia as it was the easiest to implement with regardless of minor drawbacks due to licensing features and what not.

# Game Concept
A memory game based on cards where the exciting part relies on how fast the player connects English letters with cards that represent those letters (i.e. a picture of car is the letter 'A'). 

And in the same sense, the player may be given a set of cards that represent a word and voice recognition will be used to confirm player's answer. So, for example, if the player was asked to form the word “car”, he/she will have to put the cards in order from left to right to achieve that goal. And on the other hand, the game may show the cards that form word “car” and ask the player to pronounce it, so the gameplay can go either way.

Name of the game is still discussable but `Brain Grinder` was the name that suited the concept but i changed it to `Grapheme`.

<p align="center">
<img height="265" src="Resources/game elements/playcard.jpg" />
<img height="350" src="Resources/game elements/GPF Official.png" />
<br />
Playcard           Playfield
</p>



2.3 Game Engines In Contrast With AR Litiriture Review
	Unity3D has a direct connection with ARToolkit via free packages which makes it easier to just start development with Unity3D instead of Unreal Engine which also
	got a direct pluging from ARToolkit but for 99$ so, although, the engine is free to download and use but payment must be done to develop an AR game whichautomatically
	droops it out of the choices list.

3.0 Game Designs Imparted with AR
	Majority of the games in this field requires interaction with the environment of the player with or without some predefined	images [markers], GPS locations and/or 
	sounds, so making games in that sense has to be a little bit over creative standards.

3.1 My Game Concept 
	A memory game based on cards where the exciting part lies on how fast the player connects english letters with cards that represent those letters (i.e: a picture of car is the letter 'A').
 	And in the same sense, the player maybe given a set of cards that represent a word and voice recognition will be used to confirm player's answer.

3.2 Game Modes
	- Solo : ( collect 100 stars to finish a difficulity level)
		- Frog (Easy)  : Player will be asked to form/pronounce words of 3 - 6 letters.
		- Ant (Medium) : Player will be asked to form/pronounce words of 6 letters minimum.
		- Pigeon (Hard): Player will be asked to form/pronounce sentences of 2 - 4 words.
		- Human (Intelligent): Player will be asked to form/pronounce sentences of 4 words minimum.

	- Head To Head :
		- Form Cards :
			# Input : Maximum number of words Min (3).
			# Input : Maximum number of sentences (Min 0).
			# Input : Timer on/off.
			# Input : Difficulity (Frog - Human)

		- Pronounce Words :
			# Input : Maximum number of words Min (3).
			# Input : Maximum number of sentences (Min 0).
			# Input : Timer on/off.
			# Input : Difficulity (Frog - Human).

		- Feeling Intelligent :
			# Randomized set of challenges.

	- Group vs Group : (May or may not be implemented depending on time)
		- Choose number of team members (Min 2).
		- Pick a number up (If Min then 1 or 2).
		- Randomized gameplay.
		- Every player will be given parts of the words/sentences to form/pronounce.

3.3 Game Rules & Elements

	Rules :
	- For each correct answer the player wins 1 star, maximum of 3 stars can be won depending on how fast the player forms/pronounces the letter. 
	- Every 9 stars the player gets a "Hint" helper.
	- If "Hint" is available, then it can be used by tapping on the card.
	- Player has to organize the cards from left to right to represent the given word.
	- Player has to pronounce the word represented by the cards correctly.
	- A "Timer" will be used in Head To Head or Group gameplay.

	Elements :
	- Physical Cards.
	- English words.
	- Voice recognition.
	- Leaderboard.
	- Timer.

 3.4 Platform & Device Specs
 	- Android.
 	- Kitkat 4.6 (API 19).
 	- Quad Core chip-set processor.

*********************************************** Trials *******************************************
- Natural Feature Tracking is used with images that are not easy to build as bordered markers.
- Building NFT images depends on training the camera on few images.
- Creating texture data(pattern) is easier because the generation of the data text file goes through both training and generating data files of a printed black bordered image.
 
