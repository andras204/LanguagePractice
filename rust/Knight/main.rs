use std::io;
use std::io::Write;

fn main() {

    let move_set: [i8; 8] = [-17, -15, -10, -6, 6, 10, 15, 17];
    let locked: [bool; 64] = [false;64];
    let steps: u8 = 0;

    print("Input start position between 0 and 63: ");

    let mut input_pos = String::new();
    io::stdin().read_line(&mut input_pos).unwrap();
    let mut start_pos: i8 = input_pos.trim().parse().unwrap();

    while start_pos < 0 || start_pos > 63 {
        println!("Invalid input");
        print("Input start position between 0 and 63: ");
        input_pos = String::new();
        io::stdin().read_line(&mut input_pos).unwrap();
        start_pos = input_pos.trim().parse().unwrap();
    }
    if recursive_search(start_pos, &move_set, locked, steps) == 0 {println!("success")}
    else {println!("fail")}
}

fn recursive_search(parent_pos: i8, mv_set: &[i8;8], mut lock: [bool;64], mut steps: u8) -> u8 {
    steps += 1;
    lock[parent_pos as usize] = true;
    output(&lock, steps);
    if unlocked_count(&lock) == 0 {return 0}

    let possible_moves = get_valid_moves(parent_pos, mv_set, &lock);
    if possible_moves.len() == 0 {return 1}
    let mut eval_group: Vec<(usize, usize)> = Vec::new();
    for elem in possible_moves {
        eval_group.push((elem, get_valid_moves(parent_pos + mv_set[elem], mv_set, &lock).len()));
    }
    eval_group.sort_by(|a, b| a.1.cmp(&b.1));

    for elem in eval_group {
        if recursive_search(parent_pos + mv_set[elem.0], mv_set, lock, steps) == 0 {return 0}
    }

    1
}

fn validate_move(parent_pos: i8, mv_index: usize, mv_set: &[i8;8], lock: &[bool;64]) -> bool {
    if parent_pos + mv_set[mv_index] < 0 {return false}
    if parent_pos + mv_set[mv_index] > 63 {return false}
    if lock[(parent_pos + mv_set[mv_index]) as usize] {return false}
    if mv_index / 4 == 0 {
        if mv_index % 2 == 0 {return parent_pos % 8 >= (mv_set[mv_index] % 8).abs()}
        return parent_pos % 8 < (mv_set[mv_index] % 8).abs()
    }
    if mv_index % 2 == 0 {return parent_pos % 8 >= 8 - (mv_set[mv_index] % 8).abs()}
    parent_pos % 8 < 8 - (mv_set[mv_index] % 8).abs()
}



fn get_valid_moves(parent_pos: i8, mv_set: &[i8;8], lock: &[bool;64]) -> Vec<usize> {
    let mut valid_moves: Vec<usize> = Vec::new();
    for i in 0..8 {
        if validate_move(parent_pos, i, mv_set, lock) {valid_moves.push(i)}
    }
    valid_moves
}

fn unlocked_count(lock: &[bool;64]) -> u8{
    let mut count: u8 = 0;
    for elem in lock {
        if !elem {count += 1}
    }
    count
}

fn output(lock: &[bool;64], steps: u8){
    println!("-------------------------{steps}");
    for i in 0..64 {
        if lock[i] {
            print!("{i:0>2} ");
            io::stdout().flush().unwrap()
        }
        else {print("  ")}
        if i % 8 == 7 {println!()}
    }
}

fn print(message: &str) {
    print!("{message}");
    io::stdout().flush().unwrap();
}
