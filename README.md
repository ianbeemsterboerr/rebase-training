# Rebase Training Repo

This Git repository gives you an example where you can train/experiment with Git's rebasing feature.

You can train two things here:

1. Rebase branch `rebase/feature/multiply` onto `rebase/master`.
1. Rebase and squash branch `rebase/feature/multiply` onto `rebase/master` (using interactive rebasing).

In both cases, there will (should) be a merge conflict that you need to resolve.

## Getting Started

Follow these instructions to get the repository into a state where you can train:

```
$ cd <an-empty-dir>
$ git clone -b rebase/master https://github.com/skrysmanski/rebase-training.git .
$ git checkout --track origin/rebase/feature/multiply
```

**Note:** If something went wrong, I recommend that you delete the whole directory and re-clone the repository before you try again.

## Sourcetree

### Rebase (without Squash)
Rebasing allows you to 'rebase' your changes on top of another branch or on top of new changes pushed to the branch you branched off.

1. Open the "Log / History" tab
1. Checkout **source branch** (`rebase/feature/multiply`)
1. Right-click **_head_ of target branch** (`rebase/master`) and select "Rebase..."
1. Resolve conflict via the "File Status" tab (Right-click on `Program.cs` and chose "Resolve Conflict")
1. In the menu choose "Action" -> "Continue Rebase"

### Rebase Interactively (with Squash)
Interactive rebasing allows you to squash, delete and reorder commits before rebasing them onto a different branch.

Unfortunately, Sourcetree contains an [ancient bug](https://jira.atlassian.com/browse/SRCTREEWIN-2493); so this feature doesn't fully work.

My current workaround: First do a *regular* rebase, then do the *interactive* rebase.

1. Rebase **source branch** (`rebase/feature/multiply`) on **target branch** (`rebase/master`)
1. Open the "Log / History" tab
1. Right-click the **_parent_ of oldest commit** you want to rebase interactively (here `5c3d0ea`) and select "Rebase children of xxx interactively..."
1. Modify the commits as desired (check out [this video](https://www.youtube.com/watch?v=mBCJCuU3p7I) for all the options)

### Pushing Changes

Once you're done you (probably) need to push your changes back to `origin`. Normally Git doesn't allow you to push these kinds of changes.

Instead you need to "force push" them; ideally by using `--force-with-lease` instead of `--force`. You can read about the differences between those two in [this article](https://developer.atlassian.com/blog/2015/04/force-with-lease/).

To be able to do a "force push" with Sourcetree, this **needs to be enabled in the options**.

In the menu, go to "Tools" -> "Options" -> "Git" and check "Enable Force Push". Make sure to also check "Use Safe Force Push".

Then you can select "Force Push" in "Push" dialog. (No matter the name, this will automatically use `--force-with-lease` instead of `--force` when "Use Safe Force Push" is enabled in the options.)

## CLI
### Rebase (without Squash)
Rebasing allows you to 'rebase' your changes on top of another branch or on top of new changes pushed to the branch you branched off.

1. Checkout **source branch** (`git checkout rebase/feature/multiply`)
1. Rebase **_head_ of target branch** (`git rebase rebase/master`)
1. You can optionally show the conflict in cli (`git diff`) enter `q` to exit
1. Resolve conflict in your favorite text editor
    - To see the beginning of the merge conflict in your file, search the file for the conflict marker `<<<<<<<`. When you open the file in your text editor, you'll see the changes from the HEAD or base branch after the line `<<<<<<< HEAD`. Next, you'll see `=======`, which divides your changes from the changes in the other branch, followed by `>>>>>>> BRANCH-NAME`. In this tutorial, one person wrote "`case Operator.Subtract:...`" in the base or HEAD branch and another person wrote "`case Operator.Multiply:...`" in the compare branch or `rebase/feature/multiply`. In some cases you want to keep both changes, in other cases you want to replace one of the 2 changes. If you want to keep both changes, keep an eye on the line before and after the conflict to see if you need to copy those to complete the conflict fix.
    ```
        ++<<<<<<< HEAD
        +                 case Operator.Subtract:
        +                     result = RunSubtract(operand1, operand2);
        ++=======
        +                 case Operator.Multiply:
        +                     result = RunMultiply(operand1, operand2);
        ++>>>>>>> 5062359 (Feature: Implemented multiply operation)
    ```
1. Add the fixed file (`git add .`)
1. Continue the rebase (`git rebase --continue`)
    - This will show a vim screen with the commit message, you can change the message if needed. You can quit and continue with `:q`. Look for basic vim explanation [here](https://coderwall.com/p/adv71w/basic-vim-commands-for-getting-started).

### Rebase Interactively (with Squash)
Interactive rebasing allows you to squash, delete and reorder commits before rebasing them onto a different branch.

1. Checkout **source branch** (`git checkout rebase/feature/multiply`)
1. Rebase interactively **_head_ of target branch** (`git rebase rebase/master -i`)
    - This will show a vim screen with the options and commit messages
    - You can change the action if needed in front of the commit.
        - For instance you can change an action _default `pick`_ to `f`: start edit mode in vim by pressing `i` remove `pick` from the line and enter `f`, next press `escape` to stop the edit mode in vim.
    - You can quit and write to continue with `:wq`.
    - Look for basic vim explanation [here](https://coderwall.com/p/adv71w/basic-vim-commands-for-getting-started).

### Rebase skip
Sometimes you have branched off a branch which has new or amended changes to commits which you also have in your branch. If you rebase on the new changes of this branch you will see these new or amended as conflicts in the rebasing process. In that case you can skip the current commit to continue the rebase.

1. Checkout **source branch** (`git checkout rebase/feature/refactor`)
1. Rebase **_head_ of target branch** (`git rebase rebase/feature/multiply`)
1. You will see a merge conflict on a commit which has been fixed in the `rebase/feature/multiply` branch, if you would fix the merge conflict you would end up with an empty diff. This is where `git rebase --skip` comes into play. With the skip command you will skip (basically drop) this commit from the branch you are rebasing and continue with the next commits on your branch. Since the commit is already part of the branch `rebase/feature/multiply` you don't need it anymore in your branch (`rebase/feature/refactor`).

### Rebase abort
In order to stop an ongoing rebase process you can use `git rebase --abort` to go back to the original state.

### Git reset
Sometimes it can be interesting to use `git reset ..` this undoes changes. It will remove the commits from history (or better stated point the HEAD and branch ref to a specified commit) and restore all changes to your local system showing them in your diff.

1. Checkout **source branch** (`git checkout rebase/feature/multiply`)
1. Undo last commit by using `git reset d0b2d6f` this will show the last committed files in the diff as unstaged changes. You can do some changes to the unstaged files, stage them and commit. But for now we won't.
1. Undo 2nd last commit by using `git reset 20294f3`. As in our case the first commit undoes the 2nd, it will leave the diff empty and show you can pull 2 commits from the remote. You could force push to remove both commits from the history.

In real world scenarios most likely you wouldn't end up with an empty diff, but this `git reset ..` option gives you the ability to change your commits or revert or edit some committed changes.

Optionally you can add the `--hard` option to also remove the staged changes from the undone commits.

### Git reset specific file
You can also reset a single or multiple file(s) (space separated) in a commit, doing this will leave the rest of the commit intact, and just reset the specified file(s).
You can then revert the changes to this file or change the file and amend to the existing commit.

1. Checkout **source branch** (`git checkout rebase/feature/refactor`)
1. Reset `Program.cs` `git reset 20294f3 Calculator\Calculator\Program.cs`
1. You will see `Program.cs` both as staged and unstaged file. The staged part reflects the reverted changes compared to what is in the commit and the unstaged part reflects the changes which were committed. If you would stage the unstaged changes unedited you will end up with an empty diff. If you would edit the file you would see those changes as diff if you stage them.

### Pushing Changes

Once you're done you (probably) need to push your changes back to `origin`. Normally Git doesn't allow you to push these kinds of changes.

Instead you need to "force push" them; ideally by using `--force-with-lease` instead of `--force`. You can read about the differences between those two in [this article](https://developer.atlassian.com/blog/2015/04/force-with-lease/).

So to push the changes to `origin` you can either use `git push -f` to force push or better `git push --force-with-lease` to force push with lease.

### Pulling Changes
To pull changes which were force pushed by someone else you can use `git pull --rebase`.

### Summary
A quick summary of the git commands and what they do:

- `git rebase [branch_or_commit]` rebase the current branch on the branch in the command.
- `git rebase [branch_or_commit] -i` interactively rebase the current branch on the branch in the command.
- `git rebase --continue` commit fixes on current commit and continue to the next commit on the branch which is being rebased.
- `git rebase --skip` drops the commit from the branch which is being rebased.
- `git rebase --abort` stop an ongoing rebase.

- `git reset [branch_or_commit]` undo commits and point the HEAD and branch ref to the specified branch/commit.
- `git reset [branch_or_commit] --hard` undo commits and point the HEAD and branch ref to the specified branch/commit and clear all staged changes.
- `git reset [branch_or_commit] [file_path(s)]` undo changes to a specific file and leave the rest of the commit intact.


- `git push --force-with-lease` force push with lease (check if remote has no new changes)
- `git push -f` force push
- `git pull --rebase` pull rebased changes from remote

_possible other helpful commands_
- `git log` view commit history of the current branch.
- `git status` check local repo status and show modified files.
- `git diff` shows diff of what is changed but not staged.

- `git commit -a --amend` stage and amend (add) all files to the previous commit.
- `git commit -am ""` stage and commit all files with specified commit message.

## Additional resources
- https://www.atlassian.com/git/tutorials/using-branches/merge-conflicts
- https://www.atlassian.com/git/tutorials/merging-vs-rebasing
- https://www.atlassian.com/git/tutorials/advanced-overview
- https://www.atlassian.com/git/tutorials/undoing-changes/git-reset
- https://levelup.gitconnected.com/top-30-git-commands-you-should-know-to-master-git-cli-f04e041779bc
