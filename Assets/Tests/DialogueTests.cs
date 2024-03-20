using System;
using System.Collections.Generic;
using Enums;
using LogicServices;
using LogicServices.Algorithm;
using Models;
using NUnit.Framework;

namespace Tests
{
    public class DialogueTests
    {
        [Test]
        public void SplitTextToDialogueMessages_OneSentence_ReturnsDialogueMessage()
        {
            //Given
            string testText = "This is a sentence";
            DialogueGenerationService dialogueGenerationService = new DialogueGenerationService();
            
            //When
            var result= dialogueGenerationService.SplitTextToDialogueMessages(testText, 1);
            
            //Then
            Assert.AreEqual(1,result.Length);
            Assert.AreEqual(testText,result[0].Message);
            Assert.AreEqual(1, result[0].TribeId);
        }
        
        [Test]
        public void SplitTextToDialogueMessages_MultipleSentence_ReturnsDialogueMessages()
        {
            //Given
            string testText = "This is one sentence {nm} This is second sentence {nm} and a third sentence";
            DialogueGenerationService dialogueGenerationService = new DialogueGenerationService();
            
            //When
            var result= dialogueGenerationService.SplitTextToDialogueMessages(testText, 1);
            
            //Then
            Assert.AreEqual(3,result.Length);
        }
        
        [Test]
        public void SplitTextToInstructionMessages_OneSentence_ReturnsInstructionMessage()
        {
            //Given
            string testText = "This is a sentence";
            DialogueGenerationService dialogueGenerationService = new DialogueGenerationService();
            
            //When
            var result= dialogueGenerationService.SplitTextToInstructionMessages(testText);
            
            //Then
            Assert.AreEqual(1,result.Length);
            Assert.AreEqual(testText,result[0].Message);
        }
        
        [Test]
        public void SplitTextToInstructionMessages_MultipleSentence_ReturnsInstructionMessages()
        {
            //Given
            string testText = "This is one sentence {nm} This is second sentence {nm} and a third sentence";
            DialogueGenerationService dialogueGenerationService = new DialogueGenerationService();
            
            //When
            var result= dialogueGenerationService.SplitTextToInstructionMessages(testText);
            
            //Then
            Assert.AreEqual(3,result.Length);
        }
    }
}
