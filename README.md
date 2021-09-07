<!-- links -->
<!-- https://ffmpeg-trim-and-compress.vercel.app to compress mp4 files for github -->

<!-- variables -->
<!-- [disclaimer]: src="" can't use markdown variables as in html tags as attributes placeholders or whatever -->

<!-- logo -->
<!-- <p align="center">an innovative augmented reality game that stimulates the mind</p> -->
<p align="center" href="#">
  <img align="center" height="500" src="https://user-images.githubusercontent.com/72963129/132376857-0cc8e645-4484-4f63-ab35-fda4343a482b.png" />
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
<br /><img height="50" src="https://img.shields.io/badge/Disclaimer-grey?style=for-the-badge" />

This project was done in 3 months in 2017 under emmense pressure and stress so take it with a grain of awh.

<!-- specs -->
<br /><img height="50" src="https://img.shields.io/badge/Specs-red?style=for-the-badge">

The game has only been compiled as a `.apk` so it can be played on android only for now.
- Android version > Kitkat 4.6 (API 19)
- Quad Core chip-set processor
- Rear Camera 10MP
in most cases you'll be able to play the game just fine now adays

<!-- try it -->
<br /><img height="50" src="https://img.shields.io/badge/Wanna%20Try%20It%3F-blue?style=for-the-badge">
- Download or directly print the [playfield](https://user-images.githubusercontent.com/72963129/132380746-5f23a11e-ee97-4369-a949-3f5ed0fe1c6e.png)
- Download and Install the latest `.apk` from [releases](https://github.com/onettmecha/grapheme/releases/)
- Have fun and do tell :smile:

#### I recommend you watch the [demo video](#Demo-Video) to get a firm idea of what the game is and how it's played

# Demo Video
<div align="center">
  <video loading="lazy" controls src="https://user-images.githubusercontent.com/72963129/132378306-353e3f78-cfd3-4404-b903-719443cd1ba5.mp4"></video>
</div>

---

# Content
You can read the full report [here](https://mega.nz/file/x4QCXDbB#P0JuH8iJiavT81_KToC-gL5Mq3pbBVQxu-OxO1GhI4o)
- [Introduction](#introduction)
- [Technology Review](#technology-review)
- [Game Design](#game-design)\
      - [Origin](#origin)\
      - [Design Development](#design-development)\
      - [Modes](#modes)\
      - [Rules](#rules)\
      - [Elements](#elements)
- [Sound Making](#sound-making)
- [3D Models](#3d-models)
- [Implementation](#implementation)
- [Conclusion](#conclusion)
- [Special Thanks](#special-Thanks)

# Introduction
Making a game was never an easy task because the ultimate goal is to amuse people, but, building a video game with all graphics and controllers has raised the bar a little higher. The creation of a game that interacts with player’s environment takes our imagination a little further towards the future of entertaining. This project explores a unique game concept where the phone is nothing but a middleman and whoever is playing has to move physically within the room to move objects in the game.

# Technology Review
Back in 2017, it was a little hard to find an open source AR library that fits my requirements and the devices i had at hand to construct and deliver this game and its concept. luckily there was 3 libraries and tools that gave this project a glimpse of hope and helped with my graduation project and those are:
- [Vuforia](https://developer.vuforia.com)
- [ARToolKit](http://www.artoolkitx.org)
- [IN2AR](https://www.as3gamegears.com/augmented-reality/in2ar/) \
Eventually i chose Vuforia as it was the easiest to implement with regardless of minor drawbacks due to licensing features and what not.

# Game Design
A memory game based on cards where the exciting part relies on how fast the player connects English letters with cards that represent those letters (i.e. a picture of car is the letter 'A'). 

And in the same sense, the player may be given a set of cards that represent a word and voice recognition will be used to confirm player's answer. So, for example, if the player was asked to form the word “car”, he/she will have to put the cards in order from left to right to achieve that goal. And on the other hand, the game may show the cards that form word “car” and ask the player to pronounce it, so the gameplay can go either way.

> Name of the game is discussable but `Grapheme` suits the concept for now.

<br />

### Origin

Game's proposal had some basic designs to test the development process as i was constrained by the AR library's limits on the free version. Hence the inconsistent designs as they need to fill a certain quota when uploaded to Vuforia's cloud to generate markers and so on.

|letter A|letter B |
|--------|---------|
|<img height="250" alt="LA" src="https://user-images.githubusercontent.com/72963129/132382324-369a1047-b2da-4b7a-910a-73fc719ebf3f.png" />|<img height="250" alt="LR" src="https://user-images.githubusercontent.com/72963129/132382312-8371bac5-e910-47eb-8684-00a8ad5785dd.png" />|

### Design Development
The proposed designs were simply ugly in a game that focuses on stimulating memory, so, after numerous trials the design were "fixed" or brought up to the level i imagine them by 60%. 

> During the desing of the playfield figure below it hit me that i'm not considering folks with special needs so i designed playcards in the figure below so the game would be played on a table without the physical shift of position that the game requires.

|playcard|playfield|
|--------|---------|
|<img height="280" alt="playcards" src="https://user-images.githubusercontent.com/72963129/132380716-fa04b31e-b05a-41c0-9ab8-46819e48adc2.jpg" />|<img height="360" alt="GPF Official" src="https://user-images.githubusercontent.com/72963129/132380746-5f23a11e-ee97-4369-a949-3f5ed0fe1c6e.png" />|


### Modes
<br /><img height="30" src="https://img.shields.io/badge/Solo-yellow?style=for-the-badge">

 __collect 100 stars to finish a difficulty level__ \
`Frog (Easy)` Player will be asked to form/pronounce words of 3 - 6 letters. \
`Ant (Medium)` Player will be asked to form/pronounce words of 6 letters minimum. \
`Pigeon (Hard)` Player will be asked to form/pronounce sentences of 2 - 4 words. \
`Human (Intelligent)` Player will be asked to form/pronounce sentences of 4 words minimum. 

<br /><img height="30" src="https://img.shields.io/badge/Head%20To%20Head-yellow?style=for-the-badge">

**Form Cards**
`Input` Maximum number of words Min (3). \
`Input` Maximum number of sentences (Min 0). \
`Input` Timer on/off. \
`Input` Difficulty (Frog - Human) \

**Pronounce Words**
`Input` Maximum number of words Min (3). \
`Input` Maximum number of sentences (Min 0). \
`Input` Timer on/off. \
`Input` Difficulty (Frog - Human).

**Feeling Intelligent**
`Randomized set of challenges`

<br /><img height="30" src="https://img.shields.io/badge/Group%20VS%20Group-yellow?style=for-the-badge">

__May or may not be implemented depending on time__
- Choose number of team members (Min 2).\
- Pick a number up (If Min then 1 or 2).\
- Randomized gameplay.\
- Every player will be given parts of the words/sentences to form/pronounce.

### Rules
- For each correct answer the player wins 1 star, maximum of 3 stars can be won depending on how fast the player forms/pronounces the letter.
- Every 9 stars the player gets a "Hint" helper.
- If "Hint" is available, then it can be used by tapping on the card.
- Player has to organize the cards from left to right to represent the given word.
- Player has to pronounce the word represented by the cards correctly.
- A "Timer" will be used in Head To Head or Group gameplay.

### ELEMENTS
- Physical Cards.
- English words.
- Voice recognition.
- Leader board.
- Timer.

# Sound Making
I needed to make the players feel energetic and induce the part of the brain that's responsible connecting shapes to letters. This was a great research area for me to introduce some psychology to the game concept. [Magix Music Maker Jam](https://play.google.com/store/apps/details?id=com.magix.android.mmjam&hl=en&gl=us) helped me in fulfilling the need for soundtracks for the game in spite being a bit of a hassle to deal with on the phone.

<!-- add audio samples -->

# 3D Models
Due to the short period of time and the need for 26 unique models to represent the 26 alphabets, [Microsof's 3D Builder](https://www.microsoft.com/en-us/p/3d-builder/9wzdncrfj3t6?activetab=pivot:overviewtab) and [Blender](https://www.blender.org) helped me establish modelling, testing and deploying 3d models as mobile friendly assets in very few trial and error attempts. Any of the static 3d models can either represent any letter of the alphabet in the game or a specific letter for the lower levels so they could adapt to the gameplay.

|letter A|letter B|
|--------|---------|
|<img height="280" alt="Castle" src="https://user-images.githubusercontent.com/72963129/132386806-47aa38ee-2dfb-4383-81f1-31c3d78aff8c.jpg" />|<img height="280" alt="Jaw Hex" src="https://user-images.githubusercontent.com/72963129/132386837-419dbac7-c32d-4362-926f-cbe91180e25a.jpg" />|

# Implementation
All the previous components from idea and design to implementation wouldn't be possible without [Unity3D](https://unity.com). Some areas of the development got discontinued to to meet the deadline of the prototype such as the playingcards in the figure below.

<!-- insert that figure -->

# Conclusion
This was an amazing experience for me as a developer and to explore the seas of modelling and sound making made it even better. The environmental and technical challenges i faced as a student made me question if i can even see the idea to implementation let alone deployment on mobile devices. Therefore, alhamdulelah for all insights, ideas, adaptations, power of mind and will power that i had to finish this project and graduate (class of 2017 [MMU](https://www.mmu.edu.my))

# Special Thanks
- [ffmpeg-trim-and-compress](https://github.com/Jezzabell91/ffmpeg-trim-and-compress), a great tool to compress and trim mp4 videos
- [krita](https://krita.org/en/)
- [Blender](https://www.blender.org)
