#UML Notation  
Class name - bold, capital camel case  
Variables - [-, +, #, ~] "Name" : "Data type" (camel case)  
Method/Function - [-, +, #, ~] "Name"( arg1:"Data type",..., argN:"Data type" ) : "Return type" (camel case)  
Static type - Underlined, camel case  
Constant type - ALL CAPS  

#Git Standards

##When starting a new task/branch:

1.  Make a branch from dev: `git checkout -b t_<Branch Name> dev`
2.  Do work on branch
3.  Make a commit: `git commit -a`

##To backup your branch:

1.  Save on remote: `git push -u origin t_<Branch Name>`
2.  To check if it saved your branch to the remote repo: `git branch -a`
3.  It will say: `remotes/origin/t_<Branch Name>`

##For testing:

###Your branch locally with dev:

1.  `git pull --rebase origin dev`
2.  If merging conflict(s) occur(s), See [To merge conflicts](#merge)
3.  Start testing

###Your branch with others:

####One person resets tempdev:

-  git push origin +dev:tempdev
-  Others wait until this is done

####Everyone participating in the test, merges their branch into tempdev:

1.  `git checkout tempdev`
2.  `git pull`
3.  `git merge t_<Branch Name>`
4.  `git push origin tempdev`
5.  Wait until everyone in the test has pushed
6.  `git pull origin tempdev`
7.  Start test

##Done with task/branch, ready to merge into dev:  

1.  `git checkout t_<Branch Name>`
2.  `git pull --rebase origin dev`
3.  `git checkout dev`
4.  `git merge t_<Branch Name>`
5.  `git push origin dev`

##Destroy backup copy on remote of your branch:

-  `git push origin :t_"Branch Name"`

##Destroy local copy:

1.  Make sure you are on a different branch then the one you will be deleting: `git status`
2.  If on that branch: `git checkout <Other Branch>`
3.  `git branch -d t_<Branch Name>`

Your branch will still exist in history forever, so don't fret your little nickers, child.  

##Merge well tested dev into master (remember, master must always be working):

1.  `git pull`
2.  `git checkout master`
3.  `git merge dev`
4.  `git push origin master`

##<a name="merge"/>To merge conflicts:

1.  See which files have conflicts: `git status`
2.  Open file(s) and delete and/or combine correct pieces of code between <<<< HEAD to === then to >>>>
3.  `git add <File Name>`
4.  If More files to merge repeat from step 2
5.  `git rebase --continue`

Git tags are for annotating releases and stuff